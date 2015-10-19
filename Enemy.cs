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
        float fireTimer;
        float fireDistance;
        EnemyType etype;
        Vector3 searchloc;
        EnemyController controller;

        public Enemy(LabGame game, EnemyController controller, EnemyType etype, Vector3 pos) : base()
        {
            this.game = game;
            type = GameObjectType.Enemy;
            myModel = game.assets.GetModel("player", CreateEnemyModel);
            this.pos = pos;
            this.maxspeed = 0.15f;
            this.range = 4;
            this.detected = false;
            this.controller = controller;
            this.detectrange = 3.5f;
            this.escaperange = detectrange * 2;
            this.etype = etype;
            GetParamsFromModel();
            fireTimer = 0;
            fireDistance = 4;
        }

		/// <summary>
		/// Create a new enemy model.
		/// </summary>
		/// <returns>A new enemy model.</returns>
        public MyModel CreateEnemyModel()
        {
            return game.assets.CreateShip("demoship.png");
        }

		/// <summary>
		/// Create a new enemy projectile.
		/// </summary>
		/// <returns>A new enemy projectile model.</returns>
        private MyModel CreateEnemyProjectileModel()
        {
            return game.assets.CreateCannonBall();
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            if(hitpoints <= 0)
            {
                game.score += 1;
                game.Remove(this);
            }
            int time = gameTime.ElapsedGameTime.Milliseconds;
            fireTimer -= time;
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
                    if(toPlayer.Length() <= fireDistance && fireTimer <= 0)
                    {
                        fire(toPlayer);
                        fireTimer = 3000;
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
        /// Shoot a projectile.
        /// </summary>
        public void fire(Vector2 dir)
        {
            /*dir.X *= 145;
            dir.Y *= -145;
            Vector2 position = new Vector2(-(pos.X - game.getPlayerPos().X) * 145 - 1542 / 2, (pos.Y - game.getPlayerPos().Y) * 145 + 1024 / 2);
            System.Diagnostics.Debug.WriteLine("shooter=" + this + " dirx=" + dir.X + " diry=" + dir.Y + " positionx=" + position.X + " positiony=" + position.Y + " posx=" + pos.X + " posy=" + pos.Y + " player.x=" + game.getPlayerPos().X + " player.y=" + game.getPlayerPos().Y);
            Vector3 direction = new Vector3(dir.X + position.X, -dir.Y + position.Y, 0);*/
            Vector3 direction = new Vector3(dir.X, dir.Y, 0);
            direction.Normalize();
            game.Add(new Projectile(game,
                game.assets.GetModel("shot", CreateEnemyProjectileModel),
                pos,
                direction * 10,
                this
            ));
        }


    }
}
