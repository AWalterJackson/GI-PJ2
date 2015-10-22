using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    public class Camera
    {

        public Matrix View;         //Camera view
        public Matrix Projection;   //Camera projection
        public LabGame game;        //Game
        public Vector3 pos;         //Camera position in the world
        public Vector3 oldPos;      //Camera's previous position in the world

		/// <summary>
		/// Ensures that all objects are being rendered from a consistent viewpoint
		/// </summary>
		/// <param name="game"></param>
        public Camera(LabGame game) {
            pos = new Vector3(0, 0, -10);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            this.game = game;
        }

		/// <summary>
		/// If the screen is resized, the projection matrix will change
		/// </summary>
        public void Update()
        {
            Vector3 playerpos = game.getPlayerPos();
            pos.X = playerpos.X;
            pos.Y = playerpos.Y -1 ;
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            Vector3 up = Vector3.Normalize(Vector3.Cross(playerpos - pos, Vector3.UnitX));
            View = Matrix.LookAtLH(pos, playerpos, up);
        }
    }
}
