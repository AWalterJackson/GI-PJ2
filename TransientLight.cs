using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;

namespace Project
{
    //Transient light class for explosions
    class TransientLight : GameObject
    {
        public LightingController.LightSource light;

        public TransientLight(LabGame game, Vector3 pos)
        {
            this.game = game;
            this.light.lightCol = new Vector4(0.6f, 0.6f, 0.6f, 1f);           //Yes 0.6 is that damn bright
            this.light.lightPos = new Vector4(pos.X, pos.Y, pos.Z - 2, 1f);
        }

        //Update reduces light intensity until it disappears
        public override void Update(GameTime gameTime)
        {
            this.light.lightCol *= 0.95f;
            if (this.light.lightCol.X >= 0.05f)
            {
                game.addLight(this.light);
            }
            else
            {
                game.Remove(this);
            }
        }
    }
}
