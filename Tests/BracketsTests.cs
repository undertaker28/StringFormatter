using TestClass;
using StringFormatter.Exceptions;

namespace Tests
{
    [TestClass]
    public class BracketsTests
    {
        [TestMethod]
        [ExpectedException(typeof(UnbalancedBracketsException), "Unbalanced brackets")]
        public void NoClosedBracket_ThrowsUnbalancedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, {FirstName} {LastName!", user);
        }

        [TestMethod]
        [ExpectedException(typeof(UnbalancedBracketsException), "Unbalanced brackets")]
        public void UnbalancedBrackets_ThrowsUnbalancedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, {{FirstName} {LastName}}", user);
        }

        [TestMethod]
        [ExpectedException(typeof(UnbalancedBracketsException), "Unbalanced brackets")]
        public void NoOpenedBrackets_ThrowsUnbalancedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("Привет, FirstName}}} LastName!}", user);
        }

        [TestMethod]
        [ExpectedException(typeof(UnbalancedBracketsException), "Unbalanced brackets")]
        public void WrongBracketsFormat_ThrowsUnbalancedBracketsException()
        {
            var user = new User("Павел", "Старовойтов");
            StringFormatter.StringFormatter.Shared.Format("{date {{date } date }}", user);
        }
    }
}
