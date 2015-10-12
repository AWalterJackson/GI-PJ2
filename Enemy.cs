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
    class Enemy : PhysicalObject
    {
        private float projectileSpeed = 10;

        float fireTimer;
        float fireWaitMin = 2000;
        float fireWaitMax = 20000;

        public Enemy(LabGame game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Enemy;
            myModel = game.assets.GetModel("ship", CreateEnemyModel);
            this.pos = pos;
            setFireTimer();
            GetParamsFromModel();
        }

		/// <summary>
		/// Set the enemy fire timer.
		/// </summary>
        private void setFireTimer()
        {
            fireTimer = fireWaitMin + (float)game.random.NextDouble() * (fireWaitMax - fireWaitMin);
        }

		/// <summary>
		/// Create a new enemy model.
		/// </summary>
		/// <returns>A new enemy model.</returns>
        public MyModel CreateEnemyModel()
        {
            return game.assets.CreateTexturedCube("enemy.png", 0.5f);
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
            // TASK 3: Fire projectile
            fireTimer -= gameTime.ElapsedGameTime.Milliseconds * game.difficulty;
            if (fireTimer < 0)
            {
                fire();
                setFireTimer();
            }
            basicEffect.World = Matrix.Translation(pos);
        }

		/// <summary>
		/// Fire!
		/// </summary>
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("enemy projectile", CreateEnemyProjectileModel),
                pos,
                new Vector3(0, -projectileSpeed, 0),
                GameObjectType.Player
            ));
        }

		/// <summary>
		/// React to being hit.
		/// </summary>
        public void Hit()
        {
            game.score += 10;
            game.Remove(this);
        }

    }
}
