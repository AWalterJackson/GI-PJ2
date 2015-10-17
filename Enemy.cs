using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    // Enemy class
    // Basically just shoots randomly, see EnemyController for enemy movement.
    using SharpDX.Toolkit.Graphics;

    public enum EnemyType
    {
        galleon, demoship
    }

    class Enemy : PhysicalObject
    {
        float range;
        float detectrange;
        float escaperange;
        bool detected;
        EnemyType etype;
        Vector3 searchloc;
        EnemyController controller;

        public Enemy(LabGame game, EnemyController controller, EnemyType etype, Vector3 pos) : base()
        {
            this.game = game;
            type = GameObjectType.Enemy;
            myModel = game.assets.GetModel("player", CreateEnemyModel);
            this.pos = pos;
            this.maxspeed = 0.5f;
            this.range = 4;
            this.detected = false;
            this.controller = controller;
            this.detectrange = 3.5f;
            this.escaperange = detectrange * 2;
            this.etype = etype;
            GetParamsFromModel();
        }

		/// <summary>
		/// Create a new enemy model.
		/// </summary>
		/// <returns>A new enemy model.</returns>
        public MyModel CreateEnemyModel()
        {
            return game.assets.CreateShip("boat.png");
        }

		/// <summary>
		/// Create a new enemy projectile.
		/// </summary>
		/// <returns>A new enemy projectile model.</returns>
        private MyModel CreateEnemyProjectileModel()
        {
            return game.assets.CreateTexturedCube("enemy projectile.png", new Vector3(0.2f, 0.2f, 0.4f));
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            Vector3 playerpos = game.getPlayerPos();
            Vector3 playervel = game.getPlayerVel();
            Vector2 toPlayer = new Vector2(playerpos.X-this.pos.X, playerpos.Y-this.pos.Y);
            float anglebetween = (float)Math.Acos(Vector2.Dot(new Vector2(this.velocity.X, this.velocity.Y), new Vector2(playervel.X,playervel.Y)));

            //Determines if the enemy can see the player ship or not
            if (!this.detected && toPlayer.Length() <= detectrange) {
                this.detected = true;
                this.maxspeed = 0.5f;
            }
            if (this.detected && toPlayer.Length() > escaperange)
            {
                this.detected = false;
                this.maxspeed = 0.2f;
                searchloc = controller.newSearch();
            }

            //Behaviour if player is detected
            if (this.detected)
            {
                if (this.etype == EnemyType.galleon)
                {
                    if (toPlayer.Length() <= range / 2f)
                    {
                        this.acceleration.X = -toPlayer.X;
                        this.acceleration.Y = -toPlayer.Y;
                        acceleration.Normalize();
                        acceleration /= 1.5f;
                    }
                    if (toPlayer.Length() > range / 2f)
                    {
                        this.acceleration.X = toPlayer.X;
                        this.acceleration.Y = toPlayer.Y;
                        //acceleration /= 3f;
                    }
                }
                if(this.etype == EnemyType.demoship)
                {
                    this.acceleration.X = toPlayer.X;
                    this.acceleration.Y = toPlayer.Y;
                    this.acceleration.Normalize();
                }
            }

            //Behaviour if player is not detected
            else
            {
                if (Vector3.Distance(searchloc, this.pos) < detectrange)
                {
                    searchloc = controller.newSearch();
                }

                this.acceleration.X = searchloc.X - this.pos.X;
                this.acceleration.Y = searchloc.Y - this.pos.Y;
                acceleration.Normalize();
            }

            //Handle the in-game physics
            physicsUpdate(gameTime);

            //Handle the graphics transforms for this update
            transform();
        }

		/// <summary>
		/// Fire!
		/// </summary>
        private void fire()
        {
            //throw new NotImplementedException();
        }

		/// <summary>
		/// React to being hit.
		/// </summary>
        public void Hit()
        {
            //throw new NotImplementedException();
        }

    }
}
