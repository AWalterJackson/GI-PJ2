using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // This class is optional.  It just makes it a bit easier to see what's been added, and to organise lighting separately from other code.
    public class LightingController : GameObject
    {

        //Individual light sources passed to shader
        private LightSource l1;
        private LightSource l2;
        private LightSource l3;
        private LightSource l4;
        private LightSource l5;
        private LightSource l6;
        private LightSource l7;
        private LightSource l8;
        private LightSource l9;
        private LightSource l10;
        private LightSource l11;
        private LightSource l12;
        private LightSource l13;
        private LightSource l14;
        private LightSource l15;

        /*
         * This was previously done as an array of LightSource structs with a coded limit of 15, but
         * was changed to its current form to avoid issues with zero padding arrays in HLSL
         */

        //Struct to contain lighting data in a form compatible with the HLSL shader
        public struct LightSource
        {
            public Vector4 lightPos;
            public Vector4 lightCol;
        }

        //Ambient colour value to be passed to the shader
        public Vector4 ambientCol;
        //List of lights passed to the controller from game entities
        public List<LightSource> lights;

        //Create a new controller
        public LightingController(LabGame game)
        {
            lights = new List<LightSource>();
            ambientCol = new Vector4(0.2f, 0.2f, 0.2f, 1f);

            //Initialise all lights to the "off" state, ie giving off no light.
            l1.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l1.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l2.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l2.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l3.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l3.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l4.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l4.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l5.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l5.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l6.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l6.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l7.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l7.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l8.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l8.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l9.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l9.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l10.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l10.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l11.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l11.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l12.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l12.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l13.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l13.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l14.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l14.lightCol = new Vector4(0f, 0f, 0f, 1f);
            l15.lightPos = new Vector4(0f, 0f, 0f, 1f);
            l15.lightCol = new Vector4(0f, 0f, 0f, 1f);

            //Ambient light colour is constant
            ambientCol = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);

            this.game = game;
        }

        //Method called by GameObject.draw to set the lighting parameters for a given object
        public void SetLighting(Effect effect)
        {
            effect.Parameters["lightAmbCol"].SetValue(ambientCol);

            effect.Parameters["l1"].SetValue(l1);
            effect.Parameters["l2"].SetValue(l2);
            effect.Parameters["l3"].SetValue(l3);
            effect.Parameters["l4"].SetValue(l4);
            effect.Parameters["l5"].SetValue(l5);
            effect.Parameters["l6"].SetValue(l6);
            effect.Parameters["l7"].SetValue(l7);
            effect.Parameters["l8"].SetValue(l8);
            effect.Parameters["l9"].SetValue(l9);
            effect.Parameters["l10"].SetValue(l10);
            effect.Parameters["l11"].SetValue(l11);
            effect.Parameters["l12"].SetValue(l12);
            effect.Parameters["l13"].SetValue(l13);
            effect.Parameters["l14"].SetValue(l14);
            effect.Parameters["l15"].SetValue(l15);
        }

        
        //Update internal lighting data
        public override void Update(GameTime gameTime)
        {
            //Rebuild the light variables from incoming data in lights list
            Rebuild();

            //Clear the lights list to allow for new entries to queue up
            for (int i = 0; i < lights.Count; i++)
            {
                lights.Remove(lights[i]);
            }
        }

        //Method to rebuild lighting variables
        private void Rebuild()
        {
            LightSource blanklight; //Ensure an empty light entity exists to fill empty slots
            blanklight.lightPos = new Vector4(0f, 0f, 0f, 1f);
            blanklight.lightCol = new Vector4(0f, 0f, 0f, 1f);

            //It may be ugly, but it works every time
            for (int i = 0; i < lights.Count(); i++)
            {
                if (i == 0) { l1 = lights[i]; }
                if (i == 1) { l2 = lights[i]; }
                if (i == 2) { l3 = lights[i]; }
                if (i == 3) { l4 = lights[i]; }
                if (i == 4) { l5 = lights[i]; }
                if (i == 5) { l6 = lights[i]; }
                if (i == 6) { l7 = lights[i]; }
                if (i == 7) { l8 = lights[i]; }
                if (i == 8) { l9 = lights[i]; }
                if (i == 9) { l10 = lights[i]; }
                if (i == 10) { l11 = lights[i]; }
                if (i == 11) { l12 = lights[i]; }
                if (i == 12) { l13 = lights[i]; }
                if (i == 13) { l14 = lights[i]; }
                if (i == 14) { l15 = lights[i]; }
            }

            //Fill any remaining variables with empty or "off" lights
            for (int i = lights.Count(); i < 15; i++)
            {
                if (i == 0) { l1 = blanklight; }
                if (i == 1) { l2 = blanklight; }
                if (i == 2) { l3 = blanklight; }
                if (i == 3) { l4 = blanklight; }
                if (i == 4) { l5 = blanklight; }
                if (i == 5) { l6 = blanklight; }
                if (i == 6) { l7 = blanklight; }
                if (i == 7) { l8 = blanklight; }
                if (i == 8) { l9 = blanklight; }
                if (i == 9) { l10 = blanklight; }
                if (i == 10) { l11 = blanklight; }
                if (i == 11) { l12 = blanklight; }
                if (i == 12) { l13 = blanklight; }
                if (i == 13) { l14 = blanklight; }
                if (i == 14) { l15 = blanklight; }
            }
        }

        /// <summary>
        /// Add a new light to the list.
        /// </summary>
        /// <param name="light"></param>
        public void Add(LightSource light)
        {
            lights.Add(light);
        }

        /// <summary>
        /// Remove a light from the list.
        /// </summary>
        /// <param name="light"></param>
        public void Remove(LightSource light)
        {
            lights.Remove(light);
        }
    }
}