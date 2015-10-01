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
        public Ocean(LabGame game)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 1);
            type = GameObjectType.Ocean;
            myModel = game.assets.CreateOcean(8,1);
            GetParamsFromModel();

            basicEffect.Alpha = 0.75f;
        }

        public override void Update(GameTime gametime)
        {
            return;
        }
    }
}
