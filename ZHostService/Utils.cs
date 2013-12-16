namespace ZHostService
{
    using System;
    using System.Linq;

    public static class Utils
    {
        public static byte[] ByteSubstring(byte[] msg, int from, int length)
        {
            var tmp = new byte[length];
            Array.Copy(msg, from, tmp, 0, length);

            return tmp;
        }

        public static string ByteArrayToString(byte[] message)
        {
            string ret = message.Aggregate(string.Empty, (current, b) => current + (b.ToString("X2") + " "));

            return ret.Trim();
        }  
    }
}
