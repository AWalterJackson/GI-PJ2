using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;

namespace Project
{
    using SharpDX.Toolkit.Graphics;

    /* Two enemy types, the galleon will attempt to maintain a certain distance
     * from the player while firing cannons, while the demoship will plow
     * straight into the player to deal ramming damage, dying in the process.
     */
    public enum EnemyType
    {
        galleon, demoship
    }

    class Enemy : PhysicalObject
    {
        float range;                //Range a galleon will attempt to maintain from a player
        float detectrange;          //Range at which the enemy will detect the player
        float escaperange;          //Range at which the ship will cease pursuit of the player
        bool detected;              //Variable controlling if the ship is in pursuit or not
        float fireTimer;            //Timer to stop the enemy from spamming cannonballs
        float fireDistance;         //Maximum distance an enemy will fire from
        EnemyType etype;            //Enemy type descriptor
        Vector3 searchloc;          //Location that an enemy will approach to search for a player
        EnemyController controller; //Controller responsible for this enemy

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
            searchloc = controller.newSearch(this, pos);
            this.locallight.lightCol = new Vector4(0.15f, 0f, 0f, 1f);
            this.locallight.lightPos = new Vector4(this.pos.X, this.pos.Y, this.pos.Z - 2, 1f);
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
			// Check if still alive
            if(hitpoints <= 0)
            {
                game.score += 1;
                game.Add(new TransientLight(this.game, this.pos));
                game.Remove(this);
            }

            int time = gameTime.ElapsedGameTime.Milliseconds;
            fireTimer -= time;
            Vector3 playerpos = game.getPlayerPos();
            Vector3 playervel = game.getPlayerVel();
            Vector2 toPlayer = new Vector2(playerpos.X-this.pos.X, playerpos.Y-this.pos.Y);

            //Determines if the enemy can see the player ship or not
            if (!this.detected && toPlayer.Length() <= detectrange) {
                this.detected = true;
                this.maxspeed = 0.5f;
            }

            //Determines if the player has escaped or not
            if (this.detected && toPlayer.Length() > escaperange)
            {
                this.detected = false;
                this.maxspeed = 0.2f;
                searchloc = controller.newSearch(this, pos);
            }

            //Behaviour if player is detected
            if (this.detected)
            {
                if (this.etype == EnemyType.galleon)
                {
					// Move away from the player if too close
                    if (toPlayer.Length() <= range / 2f)
                    {
                        this.acceleration.X = -toPlayer.X;
                        this.acceleration.Y = -toPlayer.Y;
                        acceleration.Normalize();
                        acceleration /= 1.5f;
                    }

                    //Move towards the player if too far
                    if (toPlayer.Length() > range / 2f)
                    {
                        this.acceleration.X = toPlayer.X;
                        this.acceleration.Y = toPlayer.Y;
                    }

					// Check if the enemy can engage the player and do so
                    if(toPlayer.Length() <= fireDistance && fireTimer <= 0)
                    {
                        fire(toPlayer);
                        fireTimer = 3000;
                    }
                }

                //I'm a demoship and I hate life, CHARGE FORWARD
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
                    searchloc = controller.newSearch(this, pos);
                }

				this.acceleration.X = searchloc.X - this.pos.X;
				this.acceleration.Y = searchloc.Y - this.pos.Y;

                acceleration.Normalize();
            }

            //Handle the in-game physics
            physicsUpdate(gameTime);
            //Handle the following light
            updateLight();
            //Handle the graphics transforms for this update
            transform();
        }
        
        /// <summary>
        /// Shoot a projectile.
        /// </summary>
        public void fire(Vector2 dir)
        {
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
