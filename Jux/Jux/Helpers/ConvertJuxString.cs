using System;
using System.Text;

namespace Jux.Helpers
{
    public class ConvertJuxString
    {
        public static string Decode(string EncodedString)
        {
            string base64Decoded;
            byte[] data = Convert.FromBase64String(EncodedString);
            base64Decoded = ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }

        public static string Encode(string DecodedString)
        {
            string base64Encoded;
            var data = ASCIIEncoding.ASCII.GetBytes(DecodedString);
            base64Encoded = Convert.ToBase64String(data);
            return base64Encoded;
        }
    }
}
