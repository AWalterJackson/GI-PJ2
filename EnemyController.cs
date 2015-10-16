using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;

    
    // Enemy Controller class.
    class EnemyController : GameObject
    {
        int round;
        Random RNGesus;
        float damageincrease;
        float armourincrease;
        // Constructor.
        public EnemyController(LabGame game)
        {
            this.game = game;
            this.round = 0;
            this.RNGesus = new Random();
            this.type = GameObjectType.None;

        }
		
		/// <summary>
		/// Set up the next wave.
		/// </summary>
        private void nextRound()
        {
            this.round++;
            createEnemies(1, 1, this.round);
        }

		/// <summary>
		/// Spawn enemies at the beginning of each round
		/// </summary>
        private void createEnemies(float dmgmod, float armmod, int numenemies)
        {
            int i = numenemies;
            Vector3 newpos;
            while (i > 0)
            {
                newpos = new Vector3(RNGesus.Next(-game.edgemax,game.edgemax),RNGesus.Next(-game.edgemax, game.edgemax),-1);
                game.gameObjects.Add(new Enemy(this.game, new Vector3(0,0,-1)));
                i--;
            }
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            if (game.Count(GameObjectType.Enemy) == 0)
            {
                nextRound();
            }
        }

		/// <summary>
		/// Move all the enemies.
		/// </summary>
        private void step()
        {
            throw new NotImplementedException();
        }
    }
}