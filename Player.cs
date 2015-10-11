using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    using System.Diagnostics;

    // Player class.
    class Player : PhysicalObject
    {
        //private float speed = 0.006f;
        private float projectileSpeed = 20;
        private float lockprogress = 0;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, 0, -5);
            velocity = new Vector3(0, 0, 0);
            acceleration = new Vector3(0, 0, 0);
            hitpoints = 1;
            armour = 1;
            maxspeed = 0.5f;
            GetParamsFromModel();
        }

		/// <summary>
		/// Create player model to render and use.
		/// </summary>
		/// <returns></returns>
        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateTexturedCube("boat.png", 0.7f);
        }

		/// <summary>
		/// Method to create projectile texture to give to newly created projectiles.
		/// </summary>
		/// <returns></returns>
        private MyModel CreatePlayerProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }
		
		/// <summary>
		/// Shoot a projectile.
		/// </summary>
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                pos,
                new Vector3(0, projectileSpeed, 0),
                GameObjectType.Enemy
            ));
        }
		
		/// <summary>
		/// Frame update.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
			// TO-DO: If the user is tapping, allow them to fire
            if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // TASK 1: Determine velocity based on accelerometer reading
            acceleration.X = (float)game.accelerometerReading.AccelerationX;
            acceleration.Y = (float)game.accelerometerReading.AccelerationY;

			// limit acceleration
            if (absAcceleration() > 1.4f)
            {
                accelerationLimiter(1.4f);
            }

            // Multiplying by acceleration.Length() means that smaller movements are quadratically smaller
            // This means there's no juddering with small input, and makes fine input control easier
            acceleration *= maxspeed * acceleration.Length();

			// Get elapsed time in seconds
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds)/1000;

			/* Change the velocity with respect to acceleration and delta.
			 */
			velocity += acceleration*time;

			// Limit velocity to some predefined value
            if (absVelocity() > maxspeed)
            {
                velocityLimiter(maxspeed);
            }

			// Get accelerometer readings
            yr = (float)game.accelerometerReading.AccelerationX;
            xr = (float)game.accelerometerReading.AccelerationY;

            // Calculate horizontal displacement and keep within the boundaries.
            if (edgeBoundingGeneric(pos.X + velocity.X*time))
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
                pos.X += velocity.X*time;
            }

			// Calculate vertical displacement and keep within the boundaries.
            if (edgeBoundingGeneric(pos.Y + velocity.Y*time))
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
                pos.Y += velocity.Y*time;
            }

			// Set the direction vector
			setDirection();

			// Apply the basicEffect transformation
            basicEffect.World = Matrix.Translation(pos) + Matrix.RotationX(xr) + Matrix.RotationY(-yr);
        }
		
		/// <summary>
		/// React to getting hit by an enemy bullet.
		/// </summary>
        public void Hit()
        {
            game.gameOver = true;
        }

		/// <summary>
		/// Initiate event upon recieving a tap input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            //fire();
        }

		// Initiate event upon recieving a swipe input
        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            //pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}