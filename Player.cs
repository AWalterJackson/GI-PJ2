using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace PirateGame
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    // Player class.
    class Player : GameObject
    {
        private float projectileSpeed = 20;

        public Player(PirateGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            //myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, game.boundaryBottom + 0.5f, 0);
            GetParamsFromModel();
        }

        //public MyModel CreatePlayerModel()
        //{
        //    return p;
        //}

        // Frame update.
        public override void Update(GameTime gameTime)
        {
        }

        public void Hit()
        {
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
        }
    }
}