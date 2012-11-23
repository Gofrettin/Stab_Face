using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.Misc
{
    public class Waypoint
    {
        private float X;
        private float Y;
        private float Z;

        public Waypoint(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public float getX()
        {
            return this.X;
        }

        public float getY()
        {
            return this.Y;
        }

        public float getZ()
        {
            return this.Z;
        }
    }
}
