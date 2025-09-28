namespace MangaReader.Utils
{
    public class ImageUtils
    {
        public const int MaxImagesSize = 10;

        private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/webp"
        };

        public static bool IsValidImageType(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return AllowedImageTypes.Contains(value);
        }
    }
}
