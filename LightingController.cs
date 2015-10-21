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
    class LightingController : GameObject
    {
        int totallights = 10;
        bool changed;

        public struct LightSource
        {
            public Vector4 lightPos;
            public Vector4 lightCol;
        }

        public Vector4 ambientCol;
        // TASKS 3 & 6: Note that an array of PackedLights has been used here, rather than the Light Class defined before.
        // This is not required, but somewhat more efficient as a single array can be passed to the shader rather than individual Lights.
        // It also makes it easier to extend the number of lights (as in Task 6)
        public List<LightSource> lights;
        private Stack<LightSource> addedlights;
        private Stack<LightSource> removedlights;

        private LightSource[] passablelights;
        public LightingController(LabGame game)
        {
            lights = new List<LightSource>();
            addedlights = new Stack<LightSource>();
            removedlights = new Stack<LightSource>();
            changed = true;

            ambientCol = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            // TASK 3: Initialise lights
            //packedLights[0].lightPos = new Vector4(-3f, 0f, 0f, 1f);
            //packedLights[0].lightCol = new Vector4(1f, 0f, 0f, 1f);

            //packedLights[1].lightPos = new Vector4(3f, 0f, 0f, 1f);
            //packedLights[1].lightCol = new Vector4(0f, 1f, 0f, 1f);

            //packedLights[2].lightPos = new Vector4(0f, 0f, -3f, 1f);
            //packedLights[2].lightCol = new Vector4(0f, 0f, 1f, 1f);

            this.game = game;
        }

        public void SetLighting(Effect effect)
        {
            // TASK 2: Pass parameters to shader
            effect.Parameters["lightAmbCol"].SetValue(ambientCol);
            effect.Parameters["lights"].SetValue(passablelights);
        }

        
        public void Update()
        {
            if (changed == true)
            {
                Rebuild();
            }
            changed = false;
        }

        private void Rebuild()
        {
            int index = 0;
            
            passablelights = new LightSource[Count()];
            foreach (LightSource light in lights)
            {
                passablelights[index] = light;
                index++;
            }
        }

        public int Count()
        {
            return lights.Count;
        }

        /// <summary>
        /// Add a new light.
        /// </summary>
        /// <param name="obj"></param>
        public void Add(LightSource light)
        {
            if (!lights.Contains(light) && !addedlights.Contains(light) && lights.Count < totallights)
            {
                changed = true;
                addedlights.Push(light);
            }
        }

        /// <summary>
        /// Remove a game object.
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(LightSource light)
        {
            if (lights.Contains(light) && !removedlights.Contains(light))
            {
                changed = true;
                removedlights.Push(light);
            }
        }

        /// <summary>
        /// Process the buffers of game objects that need to be added/removed.
        /// </summary>
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedlights.Count > 0) { lights.Add(addedlights.Pop()); }
            while (removedlights.Count > 0) { lights.Remove(removedlights.Pop()); }
        }
    }
}