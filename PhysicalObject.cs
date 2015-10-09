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
        public float heading; //Angle in radians from north

        //public abstract void Update(GameTime gametime);

        public float absVelocity()
        {
            float vx = (float)Math.Pow(this.velocity.X, 2);
            float vy = (float)Math.Pow(this.velocity.Y, 2);
            float vz = (float)Math.Pow(this.velocity.Z, 2);

            return (float)Math.Sqrt(vx + vy + vz);
        }

        public void setDirection()
        {
            throw new NotImplementedException();
            
        }

        public void velocityLimiter(float max)
        {
            this.velocity *= max / absVelocity();
        }
        public float absAcceleration()
        {
            float vx = (float)Math.Pow(this.acceleration.X, 2);
            float vy = (float)Math.Pow(this.acceleration.Y, 2);
            float vz = (float)Math.Pow(this.acceleration.Z, 2);

            return (float)Math.Sqrt(vx + vy + vz);
        }
        public void accelerationLimiter(float max)
        {
            this.acceleration *= max / absAcceleration();
        }
        public bool isColliding(PhysicalObject obj)
        {
            throw new NotImplementedException();
        }

        public bool edgeBoundingX()
        {
            if (this.pos.X >= game.edgemax)
            {
                return true;
            }
            if (this.pos.X <= -game.edgemax)
            {
                return true;
            }

            return false;
        }

        public bool edgeBoundingY()
        {
            if (this.pos.Y >= game.edgemax)
            {
                return true;
            }
            if (this.pos.Y <= -game.edgemax)
            {
                return true;
            }

            return false;
        }

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

        public void die()
        {
            game.Remove(this);
        }
    }
}
