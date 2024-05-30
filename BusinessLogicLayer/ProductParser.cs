using System.Text.RegularExpressions;
using System.Linq;

namespace BusinessLogicLayer
{
    public class ProductParser
    {
        public List<string> ParseProductName(string productName)
        {
            var attributes = new List<string>();

            if (string.IsNullOrEmpty(productName))
            {
                return attributes;
            }

            // Regular expression to match attributes separated by commas
            var checkCommaRegex = new Regex(@"(?<=,\s*)(.*?)(?=\s*(?:,|$))");
            var checkCommaMatches = checkCommaRegex.Matches(productName);

            if(checkCommaMatches.Count > 0) {
                var commaRegex = new Regex(@"(?<=^|,\s*)(.*?)(?=\s*(?:,|$))");
                var commaMatches = commaRegex.Matches(productName);
                attributes.AddRange(from Match match in commaMatches
                                    select match.Value.Trim());
            }
            
            // If no attributes were found using the comma regex, try the uppercase word and number regex
            if (attributes.Count == 0)
            {
                var uppercaseAndNumberRegex = new Regex(@"[a-zA-Z\d]+(?![a-zA-Z])");
                var uppercaseAndNumberMatches = uppercaseAndNumberRegex.Matches(productName);
                attributes.AddRange(from Match match in uppercaseAndNumberMatches
                                    select match.Value.Trim());
            }

            return attributes;
        }
    }
}