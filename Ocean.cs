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
    //Class for the water surface
    public class Ocean : GameObject
    {
        public int sealevel;
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
            this.sealevel = 1;
            myModel = game.assets.CreateOcean(degree,this.sealevel);
            GetParamsFromModel();
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gametime)
        {
            WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(World));

        }
    }
}
