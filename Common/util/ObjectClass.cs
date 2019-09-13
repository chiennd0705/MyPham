using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common.util
{
    public static class ObjectClass
    {
        static public string GetMd5Sum(string str)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            // And return it
            return sb.ToString();
        }

        static public string GetHostName(string remote_addr)
        {
            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(remote_addr);
                return iPHostEntry.HostName;
            }
            catch
            {
                return string.Empty;
            }
        }

        static public string IPAdress(string remote_addr)
        {
            //IPHostEntry iPHostEntry = Dns.GetHostEntry(remote_addr);
            //string iPAdress = iPHostEntry.AddressList.Length < 2 ? iPHostEntry.AddressList.First().ToString() : iPHostEntry.AddressList.Last().ToString();
            //return iPAdress;

            return "";
        }
    }
}