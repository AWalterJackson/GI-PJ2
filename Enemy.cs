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

        public Enemy(LabGame game, Vector3 pos) : base()
        {
            this.game = game;
            type = GameObjectType.Enemy;
            myModel = game.assets.GetModel("player", CreateEnemyModel);
            this.pos = pos;
            GetParamsFromModel();
        }

		/// <summary>
		/// Create a new enemy model.
		/// </summary>
		/// <returns>A new enemy model.</returns>
        public MyModel CreateEnemyModel()
        {
            return game.assets.CreateShip("boat.png");
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
            basicEffect.World = Matrix.Translation(pos);
        }

		/// <summary>
		/// Fire!
		/// </summary>
        private void fire()
        {
            //throw new NotImplementedException();
        }

		/// <summary>
		/// React to being hit.
		/// </summary>
        public void Hit()
        {
            //throw new NotImplementedException();
        }

    }
}
