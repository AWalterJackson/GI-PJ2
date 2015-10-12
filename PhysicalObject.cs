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

        //public abstract void Update(GameTime gametime);

		/// <summary>
		/// Get the magnitude of velocity.
		/// </summary>
		/// <returns>Magnitude of velocity.</returns>
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
            if (var >= game.edgemax)
            {
                return true;
            }
            if (var <= -game.edgemax)
            {
                return true;
            }

            return false;
        }

		/// <summary>
		/// Remove the physical object from the world.
		/// </summary>
        public void die()
        {
            game.Remove(this);
        }
    }
}
