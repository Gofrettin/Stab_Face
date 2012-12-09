using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Stab_Face.Misc
{
    [DataContractAttribute()]
    public class Waypoint
    {
        [DataMember()]
        private float X;

        [DataMember()]
        private float Y;

        [DataMember()]
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

        public float getDistance(Waypoint wp)
        {
            return (float)Math.Sqrt(Math.Pow(Math.Abs(this.getX() - wp.getX()), 2) + Math.Pow(Math.Abs(this.getY() - wp.getY()), 2));
        }

        public override String ToString() {
            return "<" + this.getX() + "> <" + this.getY() + "> <" + this.getZ() + ">";
        }
    }
}
