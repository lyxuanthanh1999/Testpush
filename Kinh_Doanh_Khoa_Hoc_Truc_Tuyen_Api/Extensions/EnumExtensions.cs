using System;
using System.ComponentModel;
using System.Globalization;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                var type = e.GetType();
                var values = Enum.GetValues(type);

                foreach (int val in values)
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val)!);
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;

                        break;
                    }
            }

            return description;
        }

        public static T ParseEnum<T>(this string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T GetValueFromDescription<T>(this string description, T defaultValue)
        {
            // typeof(T) dùng để lấy dữ liệu trả về 1 lớp và kiểm tra xem lớp đó có phải enum ko
            var type = typeof(T);
            if (!type.IsEnum)
            {
                // Nếu ko phải là lớp enum thì lấy giả trị enum mặc định
                return defaultValue;
            }
            foreach (var field in type.GetFields())
            {
                // if này để kiểm ra description ông truyền vào có phải attribute description trong enum t tạo ko nếu fai thì convert trả về attribute ko thì chỉ lấy enum trong Class Enum tui thui

                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(description);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(description);
                }
            }
            //throw new ArgumentException("Not found.", nameof(description));
            // Ko tìm thấy description thì lấy giá trị mặc định
            return defaultValue;
        }
    }
}