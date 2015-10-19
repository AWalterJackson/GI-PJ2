using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    public class Land : GameObject
    {
		int sidelength;

        public Land(LabGame game, int degree)
        {
            this.game = game;
            this.pos = new Vector3(0, 0, 0);
            type = GameObjectType.None;
            myModel = game.assets.CreateWorldBase(degree);
            GetParamsFromModel();
            
        }

        public override void Update(GameTime gametime)
        {

        }

		// Check if a point collides with the land
		public bool isColiding(Vector3 pt, float collisionRadius) {
			// Stay within boudaries
			if (pt.X <= -game.edgemax || pt.X >= game.edgemax) {
				return true;
			}
			if (pt.Y <= -game.edgemax || pt.Y >= game.edgemax) {
				return true;
			}
			// Check if this point is colliding with any point in the terrain.
			Vector3[][] map = this.myModel.modelMap;
			for (int i = 0; i < map.Length; i++) {
					for (int j = 0; i < map[i].Length;j++) {  
					// Calculate distance and return true if within collision radius
					if (Vector3.Distance(map[i][j], pt) <= collisionRadius) {
						return true;
					}
				}
			}
			return false;
		}
    }
}
