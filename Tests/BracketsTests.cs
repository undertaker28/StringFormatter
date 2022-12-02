using TestClass;

namespace Tests
{

    [TestClass]
    public class BracketsTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Unbalanced brackets")]
        public void NoClosedBracketException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, {FirstName} {LastName!", user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unbalanced brackets")]
        public void UnbalancedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, {{FirstName} {LastName}}", user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unbalanced brackets")]
        public void NoOpenedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, FirstName}}} LastName!}", user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unbalanced brackets")]
        public void WrongBracketsFormatException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("{date {{date } date }}", user);
        }
    }
}
