using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Project
{
    class Ocean : GameObject
    {
		/// <summary>
		/// Create a new ocean.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="degree"></param>
        public Ocean(LabGame game, int degree)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.Ocean;
            myModel = game.assets.CreateOcean(degree,1);
            GetParamsFromModel();

            basicEffect.Alpha = 0.75f;
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gametime)
        {
            yr = (float)game.accelerometerReading.AccelerationX;
            xr = (float)game.accelerometerReading.AccelerationY;

            //basicEffect.World = Matrix.RotationX(xr) + Matrix.RotationY(-yr);
        }
    }
}
