using System.Text.RegularExpressions;

namespace WebApplication1.Extention
{
    public static class Extention
    {
        public static string ToVnd(this double donGia)
        {
            return donGia.ToString("#,##0") + "đ";
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join("", words);
            }
            return result;
        }
        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, "á à ả ã ạ ấ ầ ẩ ẫ ậ ắ ằ ẵ ẳ ặ", "a");
            result = Regex.Replace(result, "è é ẻ ẽ ẹ ê ế ề ể ễ ệ", "e"); 
            result = Regex.Replace(result, "ò ó ỏ õ ọ ồ ố ổ ỗ ộ ơ ờ ớ ỡ ở ợ", "ο");
            result = Regex.Replace(result, "ù ú ủ ũ ụ ừ ứ ử ữ ự", "u");
            result = Regex.Replace(result, "ì í ỉ ĩ ị", "i");
            result = Regex.Replace(result, "ýỳỵýỹ", "у");
            result = Regex.Replace(result, "đ", "d");
            result = Regex.Replace(result, "[^a-z0-9-]", "");
            result = Regex.Replace(result, "(-)+", "-");
            return result;
        }
    }
}
