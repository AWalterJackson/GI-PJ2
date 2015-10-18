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

        private int coord()
        {
			// Pick a spawn spot in the upper portion of the map
            return RNGesus.Next(-game.edgemax, -game.edgemax + 
				(int)(((28.0f/128)*2*game.edgemax)*((int)Math.Pow(2, game.size)+1)));
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

        public Vector3 newSearch()
        {
            return new Vector3(coord(), coord(), -1);
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