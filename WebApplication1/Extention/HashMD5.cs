using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Extention
{
    public static class HashMD5
    {
        public static string ToMD5(this string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sbHash = new StringBuilder();
                foreach (byte b in bHash)
                    sbHash.Append(b.ToString("x2"));
                return sbHash.ToString();
            }
        }
    }
}
