using System.Text;
using System.Text.RegularExpressions;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Helpers
{
    public static class FormatData
    {
        public static string formatData(this string input, int length)
        {
            if (input.Length > length)
            {
                return input.Substring(0, length) + "...";
            }

            return input;
        }

        public static string convertToUnSign(this string s)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}