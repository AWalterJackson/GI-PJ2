using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    class Sun : PhysicalObject
    {
        //local variable declarations
        private int worldsize;
        private Vector3 ambientcolour;
        private Vector3 directionalcolour;
        private Vector3 specularcolour;
        private Vector3 lightdirection;
        private Vector3 diffusecolour;

        public Sun(LabGame game)
        {

            ambientcolour = new Vector3(0.2f, 0.2f, 0.2f);
            directionalcolour = new Vector3(0, 0, 0);
            specularcolour = new Vector3(0,0,0);
            lightdirection = new Vector3(0, 0, 0);
            diffusecolour = new Vector3(0, 0, 0);

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                LightingEnabled = true,
                View = Matrix.LookAtLH(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
                World = Matrix.Identity
            };
            this.game = game;
            worldsize = game.edgemax;
        }

        //Publically accessible light values
        public Vector3 getLightDirection(){
            return this.lightdirection;
        }

        public Vector3 getSpecular()
        {
            return this.specularcolour;
        }

        public Vector3 getAmbient()
        {
            return this.ambientcolour;
        }

        public Vector3 getDiffuse()
        {
            return this.diffusecolour;
        }

        //Update the sun's position and it's lighting angles
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            basicEffect.AmbientLightColor = new Vector3(1f, 1f, 1f);
            float sunxpos = worldsize/2 - (1.1f*worldsize/2 *(float)Math.Cos(time));
            float sunypos = -worldsize/2 * (float)Math.Sin(time);
            basicEffect.World = Matrix.Translation(sunxpos, sunypos, -5);

            //Change global lighting values
            ambientcolour = new Vector3(0.1f, 0.1f, 0.1f);
            specularcolour = new Vector3(0.1f, 0.1f, 0.166f);
            diffusecolour = new Vector3(0.6f, 0.6f, 0.6f);
            lightdirection.X = (float)Math.Cos(time);
            lightdirection.Y = (float)Math.Sin(time);
        }

    }
}
