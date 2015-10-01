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
        float prevxr;
        float prevyr;
        public Ocean(LabGame game)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.Ocean;
            myModel = game.assets.CreateOcean(2,1);
            GetParamsFromModel();

            basicEffect.Alpha = 0.75f;

            prevxr = 0;
            prevyr = 0;
        }

        public override void Update(GameTime gametime)
        {
            float yr = (float)game.accelerometerReading.AccelerationX;
            float xr = (float)game.accelerometerReading.AccelerationY;

            basicEffect.World = Matrix.RotationX(xr) + Matrix.RotationY(-yr);
        }
    }
}
