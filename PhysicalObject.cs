using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    abstract public class PhysicalObject : GameObject
    {
        public Vector3 velocity; //Speed relative to the map
        public Vector2 direction; //Unit vector in the direction of velocity
        public Vector3 acceleration; //Rate at which velocity is changing
        public int hitpoints; //Health of the object, if it goes below 0, object dies
        public float armour; //A value between 0 and 1 to indicate a percentage reduction in damage taken
        public float maxspeed; //A value that the magnitude of velocity cannot exceed
		public float maxaccel; //A value that the magnitude of acceleration cannot exceed
        public float heading; //Angle in radians from north
        public float damagemodifier; //Damage modifier (multiplicative)

        //public abstract void Update(GameTime gametime);

		/// <summary>
		/// Get the magnitude of velocity.
		/// </summary>
		/// <returns>Magnitude of velocity.</returns>
        public PhysicalObject()
        {
            this.velocity = new Vector3(0, 0, 0);
            this.direction = new Vector2(1, 0);
            this.acceleration = new Vector3(0, 0, 0);
            this.hitpoints = 1;
            this.armour = 1;
            this.maxspeed = 1;
            this.maxaccel = 1;
            this.heading = 0;
            this.damagemodifier = 1;
        }
        public float absVelocity()
        {
			return velocity.Length();
        }

		/// <summary>
		/// Set the 2-D direction vector based on the 3-D velocity vector.
		/// </summary>
        public void setDirection()
        {
            direction.X = velocity.X;
			direction.Y = velocity.Y;
			direction.Normalize();
        }

		/// <summary>
		/// Limit the magnitude of velocity to a maximum value.
		/// </summary>
		/// <param name="max"></param>
        public void velocityLimiter(float max)
        {
			if (absVelocity() > max) {
				this.velocity.Normalize();
				this.velocity*=max;
			}
        }

		/// <summary>
		/// Gets the magnitude of acceleration, which is simply the magnitude of the vector.
		/// </summary>
		/// <returns>Magnitude of acceleration. </returns>
        public float absAcceleration()
        {
			return acceleration.Length();
        }

		/// <summary>
		/// Limit the magnitude of acceleration to a maximum value
		/// </summary>
		/// <param name="max">Maximum magnitude of acceleration.</param>
        public void accelerationLimiter(float max)
        {
			if (absAcceleration() > max) {
				this.acceleration.Normalize();
				this.acceleration*=max;
			}
        }

		///<summary>
		///  Check if colliding with another object.
		/// </summary>
        public bool isColliding(PhysicalObject obj)
        {
            throw new NotImplementedException();
        }
		
		/// <summary>
		/// Check if within X boundary.
		/// </summary>
		/// <returns>True if outside X boundary.</returns>
        public bool edgeBoundingX()
        {
            return edgeBoundingGeneric(this.pos.X);
        }
		
		/// <summary>
		/// Check if within Y boundary.
		/// </summary>
		/// <returns>True if outside Y boundary.</returns>
        public bool edgeBoundingY()
        {
            return edgeBoundingGeneric(this.pos.Y);
        }

		/// <summary>
		/// Check if somevalue within square boundary.
		/// </summary>
		/// <returns>True if outside boundary.</returns>
        public bool edgeBoundingGeneric(float var)
        {
            if (var > game.edgemax)
            {
                return true;
            }
            if (var < -game.edgemax)
            {
                return true;
            }

            return false;
        }

        public bool collisionHandling(float time)
        {
            for (int i = 0; i < game.gameObjects.Count; i++)
            {
                if (game.gameObjects[i].type != GameObjectType.Enemy && game.gameObjects[i].type != GameObjectType.Player)
                {
                    continue;
                }
                if (game.gameObjects[i] == this)
                {
                    continue;
                }
                if (Vector3.Distance(pos + (velocity * time), game.gameObjects[i].pos) <= (myModel.collisionRadius + game.gameObjects[i].myModel.collisionRadius))
                {
                    if (game.gameObjects[i].GetType() != this.GetType())
                    {
                        float damage = (velocity - ((PhysicalObject)game.gameObjects[i]).velocity).Length() * 100;
                        hitpoints -= (int)damage;
                        ((PhysicalObject)game.gameObjects[i]).hitpoints -= (int)damage;
                    }

                    pos = game.gameObjects[i].pos - Vector3.Normalize(velocity) * (myModel.collisionRadius + game.gameObjects[i].myModel.collisionRadius + 0.001f);

                    velocity = velocity / -2;
                    ((PhysicalObject)game.gameObjects[i]).velocity = ((PhysicalObject)game.gameObjects[i]).velocity / -2;

                    // Other possible post collision velocity calculations

                    /*Vector3 tempdir = Vector3.Normalize(game.gameObjects[i].pos - pos);
                    velocity = -tempdir * velocity.Length();
                    ((PhysicalObject)game.gameObjects[i]).velocity = tempdir * ((PhysicalObject)game.gameObjects[i]).velocity.Length();*/

                    /*Vector3 tempdir = ((PhysicalObject)game.gameObjects[i]).velocity;
                    ((PhysicalObject)game.gameObjects[i]).velocity = velocity / 2;
                    velocity = tempdir / 2;*/
                    return true;
                }
            }
			// Land coliision handling
			if (game.worldBase.isColiding(pos, myModel.collisionRadius)) {
				// TO-DO

				
			}
            return false;
        }

        public void physicsUpdate(GameTime gameTime)
        {
            // limit acceleration
            if (absAcceleration() > maxaccel)
            {
                accelerationLimiter(maxaccel);
            }

            // Multiplying by acceleration.Length() means that smaller movements are quadratically smaller
            // This means there's no juddering with small input, and makes fine input control easier
            acceleration *= maxspeed * acceleration.Length();

            // Get elapsed time in milliseconds
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds);
            // Arbitrary speed adjucstment
            time /= 16;

            /* Change the velocity with respect to acceleration and delta.
			 */
            velocity += (acceleration) * time / 1000;

            // Limit velocity to some predefined value
            if (absVelocity() > maxspeed)
            {
                velocityLimiter(maxspeed);
            }

            if (collisionHandling(time))
            {
                return;
            }

            // Calculate horizontal displacement and keep within the boundaries.
            if (edgeBoundingGeneric(pos.X + velocity.X * time))
            {
                if (velocity.X < 0)
                {
                    pos.X = -game.edgemax;
                    velocity.X = 0f;
                }
                if (velocity.X > 0)
                {
                    pos.X = game.edgemax;
                    velocity.X = 0f;
                }
            }
            else
            {
                pos.X += velocity.X * time;
            }



            // Calculate vertical displacement and keep within the boundaries.
            if (edgeBoundingGeneric(pos.Y + velocity.Y * time))
            {
                if (velocity.Y < 0)
                {
                    pos.Y = -game.edgemax;
                    velocity.Y = 0f;
                }
                if (velocity.Y > 0)
                {
                    pos.Y = game.edgemax;
                    velocity.Y = 0f;
                }
            }
            else
            {
                pos.Y += velocity.Y * time;
            }
        }

        public void transform()
        {
            this.setDirection();

            Matrix Rotation = new Matrix(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1) * new Matrix(direction.X, direction.Y, 0, 0, -direction.Y, direction.X, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            Matrix Tilt = Matrix.RotationX(velocity.Length()/2);
            Vector3 rollSize = acceleration - Vector3.Dot(velocity, acceleration) / Vector3.Dot(velocity, velocity) * velocity;
            int rollDir = 1;
            if(Vector3.Cross(rollSize, velocity).Z > 0) { rollDir = -1; }
            Matrix playerRoll = Matrix.RotationY(rollDir*rollSize.Length());

            this.basicEffect.World = Tilt * playerRoll * Rotation * Matrix.Translation(pos);
        }

		/// <summary>
		/// Remove the physical object from the world.
		/// </summary>
        public void die()
        {
            game.Remove(this);
        }
        
        /// <summary>
        /// Method to create projectile texture to give to newly created projectiles.
        /// </summary>
        /// <returns></returns>
        public MyModel CreateProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }

        /// <summary>
        /// React to getting hit by an enemy bullet.
        /// </summary>
        public void Hit(int damage)
        {
            hitpoints -= damage;
        }

    }
}
