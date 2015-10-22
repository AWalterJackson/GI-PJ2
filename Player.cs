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

        private float projectileSpeed = 10;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("boat.png", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, 0, -1);
            this.locallight.lightCol = new Vector4(0f,0.15f,0.5f,1f);
            this.locallight.lightPos = new Vector4(this.pos.X, this.pos.Y,this.pos.Z-2,1f);
            game.addLight(this.locallight);
            velocity = new Vector3(0, 0, 0);
            acceleration = new Vector3(0, 0, 0);
            hitpoints = (int)(100 / game.difficulty);
            armour = 1;
            maxspeed = 0.5f;
            maxaccel = 1.4f;
            GetParamsFromModel();
        }

		/// <summary>
		/// Create player model to render and use.
		/// </summary>
		/// <returns></returns>
        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateShip("boat.png");
        }

		/// <summary>
		/// Frame update.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(gameTime.ElapsedGameTime.Milliseconds);
            if (hitpoints <= 0)
            {
                game.gameOver = true;
            }
            //Read acceleration from accelerometers
            acceleration.X = (float)game.accelerometerReading.AccelerationX * 1.25f;
            acceleration.Y = (float)game.accelerometerReading.AccelerationY * 1.25f;
            
            //Update player physics
            physicsUpdate(gameTime);
            //Update following light
            updateLight();
            //Perform visual transformations
            transform();
        }
        
        /// <summary>
		/// Initiate event upon recieving a tap input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            fire(new Vector2((float)args.Position.X, (float)args.Position.Y));
        }

		// Initiate event upon recieving a swipe input
        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            //Do nothing
        }

        private void fire(Vector2 dir)
        {
            Vector3 direction = new Vector3(dir.X - 1542 / 2, -dir.Y + 1024 / 2, 0);
            direction.Normalize();
            game.Add(new Projectile(game,
                game.assets.GetModel("shot", CreatePlayerProjectile),
                pos,
            direction * projectileSpeed,
                this
            ));
        }

        public MyModel CreatePlayerProjectile()
        {
            return game.assets.CreateCannonBall();
        }
    }
}