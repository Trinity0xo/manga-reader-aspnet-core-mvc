using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MangaReader.Utils
{
    public class EnumUtils
    {
        public static string GetEnumDisplayName<T>(T enumValue) where T : Enum
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).First();
            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? enumValue.ToString();
        }
    }
}
