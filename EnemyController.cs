using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace PirateGame
{
    using SharpDX.Toolkit.Graphics;

    
    // Enemy Controller class.
    class EnemyController : GameObject
    {

        // Constructor.
        public EnemyController(PirateGame game)
        {
            this.game = game;
        }

        // Set up the next wave.
        private void nextWave()
        {
        }

        // Create a grid of enemies for the current wave.
        private void createEnemies()
        {
        }

        // Frame update method.
        public override void Update(GameTime gameTime)
        {
        }

        // Return whether all enemies are dead or not.
        private bool allEnemiesAreDead()
        {
            return false;
        }

        // Method for when the game ends.
        private void gameOver()
        {
        }
    }
}