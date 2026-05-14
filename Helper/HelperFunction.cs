using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class HelperFunction
{
  public static string StringToSlug(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      return string.Empty;

    var replacements = new Dictionary<string, string>
    {
      { "áàảãạăắằẳẵặâấầẩẫậ", "a" },
      { "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "a" },
      { "éèẻẽẹêếềểễệ", "e" },
      { "ÉÈẺẼẸÊẾỀỂỄỆ", "e" },
      { "íìỉĩị", "i" },
      { "ÍÌỈĨỊ", "i" },
      { "óòỏõọôốồổỗộơớờởỡợ", "o" },
      { "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "o" },
      { "úùủũụưứừửữự", "u" },
      { "ÚÙỦŨỤƯỨỪỬỮỰ", "u" },
      { "ýỳỷỹỵ", "y" },
      { "ÝỲỶỸỴ", "y" },
      { "đ", "d" }, { "Đ", "d" },
      { "ø", "o" }, { "Ø", "o" },
      { "ß", "ss" },
      { "æ", "ae" }, { "Æ", "ae" },
      { "œ", "oe" }, { "Œ", "oe" },
      { "ð", "d" }, { "Ð", "d" },
      { "þ", "th" }, { "Þ", "th" },
      { "ł", "l" }, { "Ł", "l" }
    };
    var sbName = new StringBuilder(name);
    foreach (var kv in replacements)
      foreach (var ch in kv.Key)
        sbName.Replace(ch.ToString(), kv.Value);
    name = sbName.ToString();

    string normalized = name.Normalize(NormalizationForm.FormD);
    StringBuilder sb = new StringBuilder();
    foreach (char c in normalized)
    {
      if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        sb.Append(c);
    }

    string slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
    slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
    slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');

    return slug;
  }
}
