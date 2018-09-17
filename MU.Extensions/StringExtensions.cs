using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace MU.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNumeric(this string s)
        {
            double d = 0;
            return double.TryParse(s, out d);
        }

        public static byte[] ToBytes(this string s)
        {
            s = s.Replace(" ", "");
            if (s.Length % 2 != 0) return new byte[0];
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(int.Parse(s.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber));
            }
            return bytes;
        }

        public static string MD5(this string s)
        {
            byte[] sourcebytes = System.Text.Encoding.UTF8.GetBytes(s);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(sourcebytes);
            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}
