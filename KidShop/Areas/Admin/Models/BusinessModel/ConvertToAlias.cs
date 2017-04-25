using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.BusinessModel
{
    public class ConvertToAlias
    {
        private static string[] a = new string[] { "à", "á", "ạ", "ả", "ã", "â", "ầ", "ấ", "ậ", "ẩ", "ẫ", "ă", "ắ", "ằ", "ắ", "ặ", "ẳ", "ẵ", "a" };
        private static string[] d = new string[] { "đ", "d" };
        private static string[] e = new string[] { "è", "é", "ẹ", "ẻ", "ẽ", "ê", "ề", "ế", "ệ", "ể", "ễ", "e" };
        private static string[] ii = new string[] { "ì", "í", "ị", "ỉ", "ĩ", "i" };
        private static string[] y = new string[] { "ỳ", "ý", "ỵ", "ỷ", "ỹ", "y" };
        private static string[] o = new string[] { "ò", "ó", "ọ", "ỏ", "õ", "ô", "ồ", "ố", "ộ", "ổ", "ỗ", "ơ", "ờ", "ớ", "ợ", "ở", "ỡ", "o" };
        private static string[] u = new string[] { "ù", "ú", "ụ", "ủ", "ũ", "ừ", "ứ", "ự", "ử", "ữ", "u", "ư" };
        public static string ConvertTitle(string Name)
        {
            string result = "";
            string currentchar = "";
            Name = Name.Replace(" ", "-");
            Name = Name.Replace(")", "");
            Name = Name.Replace("(", "");
            Name = Name.Replace("*", "");
            Name = Name.Replace("[", "");
            Name = Name.Replace("]", "");
            Name = Name.Replace("}", "");
            Name = Name.Replace("{", "");
            Name = Name.Replace(">", "");
            Name = Name.Replace("<", "");
            Name = Name.Replace("=", "");
            Name = Name.Replace(":", "");
            Name = Name.Replace(",", "");
            Name = Name.Replace("'", "");
            Name = Name.Replace("\"", "");
            Name = Name.Replace("/", "");
            Name = Name.Replace("\\", "");
            Name = Name.Replace("&", "");
            Name = Name.Replace("?", "");
            Name = Name.Replace(";", "");
            Name = Name.ToLower();
            int len = Name.Length;
            if (Name.Length > 0)
            {
                int i;
                for (i = 0; i < len; i++)
                {
                    currentchar = Name.Substring(i, 1);
                    result = result + ChangeChar(currentchar);
                }
            }
            else
            {
                result = "";
            }
            return result;
        }
        public static string ChangeChar(string charinput)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Equals(charinput))
                {
                    return "a";
                }
            }
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i].Equals(charinput))
                {
                    return "d";
                }
            }
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i].Equals(charinput))
                {
                    return "e";
                }
            }
            for (int i = 0; i < ii.Length; i++)
            {
                if (ii[i].Equals(charinput))
                {
                    return "i";
                }
            }
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i].Equals(charinput))
                {
                    return "y";
                }
            }
            for (int i = 0; i < o.Length; i++)
            {
                if (o[i].Equals(charinput))
                {
                    return "o";
                }
            }
            for (int i = 0; i < u.Length; i++)
            {
                if (u[i].Equals(charinput))
                {
                    return "u";
                }
            }
            return charinput;
        }
    }
}