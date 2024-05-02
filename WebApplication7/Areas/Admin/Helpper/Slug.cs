using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace WebApplication7.Areas.Admin.Helpper
{
    public static class Slug
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Loại bỏ các ký tự đặc biệt, thay thế khoảng trắng bằng dấu gạch ngang
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    if (c == ' ')
                    {
                        stringBuilder.Append('-');
                    }
                    else if (c == '-')
                    {
                        // Đảm bảo không có dấu gạch ngang liên tiếp
                        if (stringBuilder.Length == 0 || stringBuilder[stringBuilder.Length - 1] != '-')
                        {
                            stringBuilder.Append(c);
                        }
                    }
                    else if ((int)c >= 97 && (int)c <= 122)
                    {
                        stringBuilder.Append(c);
                    }
                    else if ((int)c >= 65 && (int)c <= 90)
                    {
                        stringBuilder.Append(char.ToLower(c));
                    }
                    else if (c == 'Đ' || c == 'đ')
                    {
                        stringBuilder.Append('d');
                    }
                }
            }

            // Loại bỏ dấu gạch ngang ở đầu và cuối chuỗi
            string slug = stringBuilder.ToString().Trim('-');

            // Loại bỏ các dấu gạch ngang liên tiếp
            slug = Regex.Replace(slug, @"-{2,}", "-");

            return slug;
        }

    }
}
