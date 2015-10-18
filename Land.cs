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
			Vector3[][] map = this.myModel.modelMap;
			for (int i = 0; i < map.Length; i++) {
					for (int j = 0; i < map[i].Length;j++) {  
					// Check if point collides
					if (Vector3.Distance(map[i][j], pt) <= collisionRadius) {
						return true;
					}
				}
			}
			return false;
		}
    }
}
