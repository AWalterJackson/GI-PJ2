using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    // Projectile classed, used by both player and enemy.
    class Projectile : GameObject
    {
        private Vector3 vel;
        private PhysicalObject shooter;	// The physical object that fired this projectile
        private float hitRadius = 0.5f;
        private float squareHitRadius;
		
		/// <summary>
		/// Create a new projectile.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="myModel"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="targetType"></param>
        public Projectile(LabGame game, MyModel myModel, Vector3 pos, Vector3 vel, PhysicalObject shooter)
        {
            this.game = game;
            this.myModel = myModel;
            this.pos = pos;
            this.vel = vel;
            this.shooter = shooter;
            squareHitRadius = hitRadius * hitRadius;
            GetParamsFromModel();
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            // Apply velocity to position.
            pos += vel * timeDelta;

            // Set local transformation to be spinning according to time for fun.
            basicEffect.World = Matrix.RotationY(time) * Matrix.RotationZ(time * time) * Matrix.Translation(pos);

            // Check if collided with the target type of object.
            checkForCollisions();
        }

		/// <summary>
		/// Check if collided with the target type of object.
		/// </summary>
        private void checkForCollisions()
        {
            foreach (var obj in game.gameObjects)
            {
                if (obj != shooter && ((((GameObject)obj).pos - pos).LengthSquared() <= 
                    Math.Pow(((GameObject)obj).myModel.collisionRadius + this.myModel.collisionRadius, 2)))
                {
                    // Cast to object class and call Hit method.
                    switch (obj.type)

                    {
                        case GameObjectType.Player:
                            ((Player)obj).Hit();
                            break;
                        case GameObjectType.Enemy:
                            ((Enemy)obj).Hit();
                            break;
                    }

                    // Destroy self.
                    game.Remove(this);
                }
            }
        }
    }
}
