namespace TestClass
{
    class User
    {
        public string FirstName { get; }
        public string LastName { get; }

        public int _id;

        public string str1 = "str2";
        public string str2 = "str1";

        public DateTime date;

        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            _id = 1;
            date = new DateTime(2022, 12, 13);
        }

        public string GetGreeting()
        {
            return StringFormatter.StringFormatter.Shared.Format(
                "Привет, {FirstName} {LastName}!", this);
        }
    }
}
