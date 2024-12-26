using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Birthday
{
    public  class DateKeywordParser
    {
        public DateTime Parse(string input)
        {
            // Handle date like NOW-1d, NOW+2h
            if (input.Contains("-") || input.Contains("+"))
            {
                return ParseRelativeOffset(input);
            }

            // Handle date like NOW, YESTERDAY, TODAY
            return GetDateFromKeyword(input);
        }

        public string Format(string pattern, CultureInfo culture = null)
        {
            // Handle specific formats like Format(NOW, "yyyy-MM-dd")
            var match = Regex.Match(pattern, @"Format\(([^,]+),\s*""([^""]+)""\)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string keyword = match.Groups[1].Value;
                string format = match.Groups[2].Value;
                DateTime date = Parse(keyword);
                return date.ToString(format, culture ?? CultureInfo.InvariantCulture);
            }

            throw new ArgumentException($"Invalid format pattern: {pattern}");
        }

        private DateTime ParseRelativeOffset(string input)
        {
            var match = Regex.Match(input, @"^(NOW|TODAY|YESTERDAY)([-+]\d+[dhms])$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid relative offset: {input}");
            }

            string baseKeyword = match.Groups[1].Value;
            string offset = match.Groups[2].Value;

            DateTime baseDate = GetDateFromKeyword(baseKeyword);
            int value = int.Parse(offset.Substring(1, offset.Length - 2));
            char unit = offset[^1];

            return unit switch
            {
                'd' => offset[0] == '+' ? baseDate.AddDays(value) : baseDate.AddDays(-value),
                'h' => offset[0] == '+' ? baseDate.AddHours(value) : baseDate.AddHours(-value),
                'm' => offset[0] == '+' ? baseDate.AddMinutes(value) : baseDate.AddMinutes(-value),
                's' => offset[0] == '+' ? baseDate.AddSeconds(value) : baseDate.AddSeconds(-value),
                _ => throw new ArgumentException($"Invalid time unit in offset: {unit}")
            };
        }

        private DateTime GetDateFromKeyword(string keyword)
        {
            return keyword.ToUpper() switch
            {
                "NOW" => DateTime.Now,
                "YESTERDAY" => DateTime.Now.AddDays(-1),
                "TODAY" => DateTime.Today,
                _ => throw new ArgumentException($"Unsupported keyword: {keyword}")
            };
        }
    }
}
