namespace StringFormatter
{
    internal interface IStringFormatter
    {
        string Format(string template, object target);
    }
}
