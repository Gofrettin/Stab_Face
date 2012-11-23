using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.Misc
{
    public static class Utils
    {
        public static String byteArrToHexString(byte[] ba)
        {
            //ba = ba.Reverse().ToArray();
            String hex = "";
            foreach (byte b in ba)
            {

                hex += b.ToString("X2");
            }
            return hex;
        }

        public static String byteArrToString(byte[] ba)
        {
            ba = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, ba);
            return Encoding.UTF8.GetString(ba, 0, ba.Length);
        }

        public static String hexToString(String hexString)
        {
            String readable = "";
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                readable += (Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return readable;
        }
    }
}
