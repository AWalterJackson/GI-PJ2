using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    class Light
    {
        public Vector4 lightcol;
        public Vector4 lightpos;

        public Light()
        {
            lightcol = new Vector4(0f, 0f, 0f, 1f);
            lightpos = new Vector4(0f, 0f, 0f, 1f);
        }
    }
}