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
        public Random RNGesus;
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

        private int coord()
        {

            return RNGesus.Next(-game.edgemax, game.edgemax);

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
                newpos = new Vector3(coord(),coord(),-1);
				game.gameObjects.Add(new Enemy(this.game, this, EnemyType.galleon, newpos));
				newpos = new Vector3(coord(),coord(),-1);
				game.gameObjects.Add(new Enemy(this.game, this, EnemyType.demoship, newpos));
                i--;
            }
        }

        public Vector3 newSearch(Vector3 curPos)
        {
            return new Vector3(curPos.X + 5*RNGesus.NextFloat(-1,1), curPos.Y + 5*RNGesus.NextFloat(-1,1), curPos.Z);
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
    }
}