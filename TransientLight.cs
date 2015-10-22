﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;

namespace Project
{
    class TransientLight : GameObject
    {
        public LightingController.LightSource light;

        public TransientLight(LabGame game, Vector3 pos)
        {
            this.game = game;
            this.light.lightCol = new Vector4(1f, 1f, 1f, 1f);
            this.light.lightPos = new Vector4(pos.X, pos.Y, pos.Z - 2, 1f);
            game.addLight(this.light);
        }

        public override void Update(GameTime gameTime)
        {
            game.removeLight(this.light);
            this.light.lightCol *= 0.95f;
            if (this.light.lightCol.X >= 0.05f)
            {
                game.addLight(this.light);
            }
            else
            {
                game.removeLight(this.light);
                game.Remove(this);
            }
        }
    }
}
