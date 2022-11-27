namespace StringFormatter
{
    internal class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();
        private ReflectionHelper _reflectionHelper;
        private StringFormatter()
        {
            _reflectionHelper = new ReflectionHelper();
        }
        /*
                public static Func<object, object> CreateGetter(object entity, string propertyName)
                {
                    var param = Expression.Parameter(typeof(object), "e");
                    Expression body = Expression.PropertyOrField(Expression.TypeAs(param, entity.GetType()), propertyName);
                    var getterExpression = Expression.Lambda<Func<object, object>>(body, param);
                    return getterExpression.Compile();
                }*/
        public string Format(string template, object target)
        {
            bool result = isEqualBrackets(template);
            int startId, endId;
            findSubstring(template, 0, out startId, out endId);
            var resultedString = template;
            while (startId != endId)
            {
                int SubstringId;
                resultedString = changeFieldTostring(target, resultedString, startId, endId, out SubstringId);
                findSubstring(resultedString, SubstringId, out startId, out endId);
            }

            return resultedString;
        }

        private bool isEqualBrackets(string template)
        {
            int openedBrackets = 0;
            int closedBrackets = 0;
            foreach (var item in template.ToCharArray())
            {
                if (item == '{')
                {
                    openedBrackets++;
                }
                else if (item == '}')
                {
                    closedBrackets++;
                    if (closedBrackets > openedBrackets)
                    {
                        return false;
                    }
                    openedBrackets--;
                    closedBrackets--;
                }
            }
            return (openedBrackets == 0 && closedBrackets == 0);
        }

        private string changeFieldTostring(object obj, string str, int startId, int endId, out int endOfChangedSubstring)
        {
            //get string in brackets;
            var field = str.Substring(startId, endId - startId);
            field = field.Trim(' ');

            var value = _reflectionHelper.GetPropertyValue(obj, field);


            //field = field.Replace(" ", String.Empty);

            // Type type = obj.GetType();
            // var fieldsInfo = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            // object value = null ;
            /*  foreach (var fieldInfo in fieldsInfo)
              {
                  if (fieldInfo.Name == field) { 
                      value = fieldInfo.GetValue(obj);
                      break;
                  }
              }*/
            string result = str;
            if (value != null)
            {
                var subStringLength = endId - startId + 2;
                string replacedString = str.Substring(startId - 1, subStringLength);
                result = str.Replace(replacedString, value.ToString());
                endOfChangedSubstring = endId - (subStringLength - value.ToString().Length - 1);
                //value.ToString();
            }
            else
            {
                endOfChangedSubstring = endId;
            }
            // string result = str.Replace(field, "C#");
            // var propertiesInfo = type.GetProperties();

            return result;
        }


        private void findSubstring(string str, int startedFrom, out int startId, out int endId)
        {
            bool isFoundBoundaries = false;
            startId = str.IndexOf("{", startedFrom) + 1;
            int secondStart;
            endId = 0;
            while (isFoundBoundaries == false && startId != 0)
            {
                endId = str.IndexOf("}", startId);
                secondStart = str.IndexOf("{", startId) + 1;
                if (secondStart < endId && secondStart != 0)
                {
                    startId = secondStart;
                }
                else
                {
                    isFoundBoundaries = true;
                }
            }

            //int secondStart = str.IndexOf("{", start)+1;


            //string result = s.Substring(start, end - start); 
            // return str;
        }
    }
}
