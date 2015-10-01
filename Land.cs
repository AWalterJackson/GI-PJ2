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
        public Land(LabGame game)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.None;
            myModel = game.assets.CreateWorldBase(9);
            GetParamsFromModel();
        }

        public override void Update(GameTime gametime)
        {
            return;
        }
    }
}
