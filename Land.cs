using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    class Land : GameObject
    {
        float prevxr;
        float prevyr;
        public Land(LabGame game)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.None;
            myModel = game.assets.CreateTexturedCube("player.png",1);
            GetParamsFromModel();
            prevxr = 0;
            prevyr = 0;
            
        }

        public override void Update(GameTime gametime)
        {
            float yr = (float)game.accelerometerReading.AccelerationX;
            float xr =  (float)game.accelerometerReading.AccelerationY;

            //if(xr-this.prevxr == 0)

            basicEffect.World = Matrix.RotationX(xr)+Matrix.RotationY(-yr);

            //this.prevxr = xr;
            //this.prevyr = yr;
        }
    }
}
