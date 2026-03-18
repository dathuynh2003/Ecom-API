using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Ecom.Shared.Utils
{
    public class SlugUtils
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return string.Empty;
            // Normalize and remove diacritics (accents)
            string normalized = phrase.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            string noAccents = sb.ToString().Normalize(NormalizationForm.FormC);
            // Convert to lowercase
            string lower = noAccents.ToLowerInvariant();
            // Remove invalid characters (keep letters, numbers, spaces, and hyphens)
            string cleaned = Regex.Replace(lower, @"[^a-z0-9\s-]", "");
            // Replace multiple spaces or hyphens with a single space
            string singleSpaced = Regex.Replace(cleaned, @"[\s-]+", " ").Trim();
            // Replace spaces with hyphens
            string slug = Regex.Replace(singleSpaced, @"\s", "-");
            return slug;
        }
    }
}
