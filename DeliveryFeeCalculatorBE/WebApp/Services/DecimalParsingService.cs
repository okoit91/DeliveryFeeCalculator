using System.Globalization;

namespace WebApp.Services
{
    public class DecimalParsingService
    {
        public bool TryParse(string? input, out decimal result)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return decimal.TryParse(input.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result);
            }

            result = 0;
            return false;
        }

        public bool TryParseMultiple(Dictionary<string, string?> inputs,
            out Dictionary<string, decimal> results, out List<string> invalidKeys)
        {
            results = new Dictionary<string, decimal>();
            invalidKeys = new List<string>();

            foreach (var input in inputs)
            {
                if (TryParse(input.Value, out decimal value))
                {
                    results.Add(input.Key, value);
                }
                else
                {
                    invalidKeys.Add(input.Key);
                }
            }

            return invalidKeys.Count == 0;
        }
    }
}