namespace StringFormatter
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new();
        private readonly ReflectionHelper reflectionHelper;
        private StringFormatter() => reflectionHelper = new ReflectionHelper();
        public string Format(string template, object target)
        {
            if (!IsEqualBrackets(template))
                throw new Exception("Unbalanced brackets");

            var substrings = template.Split(new string[] { "{{" }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < substrings.Length; i++)
                substrings[i] = FormatSubstring(substrings[i], target);

            string result = string.Join("{", substrings);
            if (template.IndexOf("{{") == 0)
                result = "{" + result;

            if (template.LastIndexOf("{{") == template.Length - 2)
                result = result + "{";
            result = result.Replace("}}", "}");

            return result;
        }

        private string FormatSubstring(string template, object target)
        {
            if (!IsEqualBrackets(template))
                throw new Exception("Unbalanced brackets");

            FindSubstring(template, 0, out int startId, out int endId);
            var resultedString = template;
            while (startId != endId)
            {
                resultedString = ChangeFieldTostring(target, resultedString, startId, endId, out int SubstringId);
                FindSubstring(resultedString, SubstringId, out startId, out endId);
            }

            return resultedString;
        }

        private bool IsEqualBrackets(string _template)
        {
            string template = _template.Replace("{{", "");
            template = template.Replace("}}", "");

            int openedBrackets = 0;
            int closedBrackets = 0;
            foreach (var item in template.ToCharArray())
            {
                if (item == '{')
                    openedBrackets++;
                else if (item == '}')
                {
                    closedBrackets++;
                    if (closedBrackets > openedBrackets)
                        return false;
                    openedBrackets--;
                    closedBrackets--;
                }
            }

            return (openedBrackets == 0 && closedBrackets == 0);
        }

        private string ChangeFieldTostring(object obj, string str, int startId, int endId, out int endOfChangedSubstring)
        {
            var field = str.Substring(startId, endId - startId);
            field = field.Trim(' ');

            var value = reflectionHelper.GetPropertyValue(obj, field);

            string result = str;
            if (value != null)
            {
                var subStringLength = endId - startId + 2;
                string replacedString = str.Substring(startId - 1, subStringLength);
                result = str.Replace(replacedString, value.ToString());
                endOfChangedSubstring = endId - (subStringLength - value.ToString().Length - 1);
            }
            else
                endOfChangedSubstring = endId;

            return result;
        }

        private void FindSubstring(string str, int startedFrom, out int startId, out int endId)
        {
            bool isFoundBoundaries = false;
            startId = str.IndexOf("{", startedFrom) + 1;

            int secondStart;
            endId = 0;
            while (isFoundBoundaries == false && startId != 0)
            {
                endId = str.IndexOf("}", startId);
                while (endId > 0 && endId < str.Length && str.IndexOf("}}", endId) == endId)
                    endId = str.IndexOf("}", endId + 1);
                secondStart = str.IndexOf("{", startId) + 1;

                if (secondStart < endId && secondStart != 0)
                    startId = secondStart;
                else
                    isFoundBoundaries = true;
            }
        }
    }
}
