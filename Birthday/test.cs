using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class DateKeywordParser
{
    public DateTime Parse(string input)
    {
        if (input.StartsWith("Format("))
        {
            return ParseFormattedDate(input);
        }

        return ParseKeyword(input);
    }

    private DateTime ParseKeyword(string keyword)
    {
        Regex offsetRegex = new(@"^(NOW|YESTERDAY|TODAY)([+-]\d+[dhms])?$", RegexOptions.IgnoreCase);
        Match match = offsetRegex.Match(keyword);

        if (!match.Success)
            throw new ArgumentException("Invalid keyword or format");

        string baseKeyword = match.Groups[1].Value.ToUpper();
        string offset = match.Groups[2].Value;

        DateTime baseDate = baseKeyword switch
        {
            "NOW" => DateTime.Now,
            "YESTERDAY" => DateTime.Now.AddDays(-1),
            "TODAY" => DateTime.Today,
            _ => throw new ArgumentException("Unsupported keyword")
        };

        return ApplyOffset(baseDate, offset);
    }

    private DateTime ApplyOffset(DateTime baseDate, string offset)
    {
        if (string.IsNullOrEmpty(offset)) return baseDate;

        char unit = offset[^1];
        int value = int.Parse(offset[..^1]);

        return unit switch
        {
            'd' => baseDate.AddDays(value),
            'h' => baseDate.AddHours(value),
            'm' => baseDate.AddMinutes(value),
            's' => baseDate.AddSeconds(value),
            _ => throw new ArgumentException("Invalid offset unit")
        };
    }

    private DateTime ParseFormattedDate(string input)
    {
        Regex formatRegex = new(@"Format\(([^,]+),\s*""([^""]+)""\)");
        Match match = formatRegex.Match(input);

        if (!match.Success)
            throw new ArgumentException("Invalid format syntax");

        string baseKeyword = match.Groups[1].Value.Trim();
        string format = match.Groups[2].Value;

        DateTime baseDate = ParseKeyword(baseKeyword);
        return DateTime.ParseExact(baseDate.ToString(format, CultureInfo.InvariantCulture), format, CultureInfo.InvariantCulture);
    }

    public string FormatDate(DateTime date, string format, CultureInfo culture)
    {
        return date.ToString(format, culture);
    }
}
