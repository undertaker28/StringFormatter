using StringFormatter.TestClasses;

namespace StringFormatter
{
    class Program
    {
        static void Main(string[] args)
        {

            var obj = new Class1();
            string result;

            result = StringFormatter.Shared.Format("{hello world}", obj);
            result = StringFormatter.Shared.Format("{str}{world}", obj);
            result = StringFormatter.Shared.Format("{{str}{i}}", obj);
            result = StringFormatter.Shared.Format("{hello world}}", obj);
            result = StringFormatter.Shared.Format("{hello world}{}", obj);

            Console.WriteLine("Hello, World!");
        }
    }
}
