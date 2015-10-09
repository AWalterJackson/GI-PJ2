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

        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateTexturedCube("boat.png", 0.7f);
        }

        // Method to create projectile texture to give to newly created projectiles.
        private MyModel CreatePlayerProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }

        // Shoot a projectile.
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                pos,
                new Vector3(0, projectileSpeed, 0),
                GameObjectType.Enemy
            ));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // TASK 1: Determine velocity based on accelerometer reading
            acceleration.X = (float)game.accelerometerReading.AccelerationX;
            acceleration.Y = (float)game.accelerometerReading.AccelerationY;

            if (absAcceleration() > 1.4f)
            {
                accelerationLimiter(1.4f);
            }

            // Multiplying by acceleration.Length() means that smaller movements are quadratically smaller
            // This means there's no juddering with small input, and makes fine input control easier
            acceleration *= maxspeed * acceleration.Length();

            int time = gameTime.ElapsedGameTime.Milliseconds;
            // Fine details on multipliers such as /1000 below can be ironed out as the game progresses, this was just a nice number that made the controls about right
            velocity += (acceleration - velocity)*time/1000;
            /*Other method, independant of time, both are good:
            velocity = acceleration;
            Try them out and tell me what you think, but I like the momentum effect that multiplying by time gives.
            They are esentially the same, just with and without time; as "velocity = acceleration" can be written as "velocity += (acceleration - velocity)*1" instead of *time */

            if (absVelocity() > maxspeed)
            {
                velocityLimiter(maxspeed);
            }

            yr = (float)game.accelerometerReading.AccelerationX;
            xr = (float)game.accelerometerReading.AccelerationY;

            // Keep within the boundaries.
            if (edgeBoundingGeneric(pos.X + velocity.X))
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
                pos.X += velocity.X;
            }

            if (edgeBoundingGeneric(pos.Y + velocity.Y))
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
                pos.Y += velocity.Y;
            }
            //setDirection();

            basicEffect.World = Matrix.Translation(pos) + Matrix.RotationX(xr) + Matrix.RotationY(-yr);
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.gameOver = true;
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            //fire();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            //pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}