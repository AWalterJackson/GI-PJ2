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
        int round;              //Round counter
        public Random RNGesus;  //RNG for AI seeking behaviour

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

            //Limit maximum number of enemies... have to make at least not impossible
            if (i > 7)
            {
                i = 7;
            }

            //Spawn two enemies for every round (Up to a maximum of  14 enemies)
            while (i > 0)
            {
                newpos = new Vector3(coord(),coord(),-1);
				game.gameObjects.Add(new Enemy(this.game, this, EnemyType.galleon, newpos));
				newpos = new Vector3(coord(),coord(),-1);
				game.gameObjects.Add(new Enemy(this.game, this, EnemyType.demoship, newpos));
                i--;
            }
        }

        //Pick a new search location to hunt for the player
        public Vector3 newSearch(Enemy ship, Vector3 curPos)
        {
            Vector3 loc = new Vector3(coord(), coord(), curPos.Z);
            while (!hasValidPath(ship, loc))    //If the new search location has terrain blocking it, find a new one
            {
                loc = new Vector3(coord(), coord(), curPos.Z);
            }
            return loc;
        }

		/// <summary>
		/// Update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            //If all the enemies are dead, time to move on
            if (game.Count(GameObjectType.Enemy) == 0)
            {
                nextRound();
            }
        }

        //Function to check if the given enemy has a path to its search location
        private bool hasValidPath(Enemy ship, Vector3 point)
        {
            Vector2 topoint = new Vector2(point.X - ship.pos.X, point.Y - ship.pos.Y);
            Vector2 testpoint = new Vector2(ship.pos.X, ship.pos.Y);
            for (float i = 0.01f; i < 1; i += 0.01f)
            {
                testpoint.X = (int)(ship.pos.X + topoint.X * i) + game.edgemax;
                testpoint.Y = (int)(ship.pos.Y + topoint.Y * i) + game.edgemax;

                if (game.worldBase.heights[(int)testpoint.X][(int)testpoint.Y].Z < -game.ocean.sealevel)
                {
                    return false;
                }
            }
            return true;
        }
    }
}