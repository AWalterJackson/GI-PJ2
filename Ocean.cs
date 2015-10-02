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
        public Ocean(LabGame game, int degree)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.Ocean;
            myModel = game.assets.CreateOcean(degree,1);
            GetParamsFromModel();

            basicEffect.Alpha = 0.75f;
        }

        public override void Update(GameTime gametime)
        {
            yr = (float)game.accelerometerReading.AccelerationX;
            xr = (float)game.accelerometerReading.AccelerationY;

            basicEffect.World = Matrix.RotationX(xr) + Matrix.RotationY(-yr);
        }
    }
}
