using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        public Stack<string> _stack;

        [SetUp]
        public void SetUp()
        {
            _stack = new Stack<string>();
        }

        [Test]
        public void Count_EmptyStack_ReturnZero()
        {
            Assert.That(_stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Push_ArgumentIsNull_ReturnArgumentNullException()
        {
            Assert.That(() => _stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Push_ArgumentIsValid_ShouldAddObjectToTheStack()
        {
            _stack.Push("Teste");

            Assert.That(_stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Pop_EmptyStack_ReturnInvalidOperationException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_StackWithAFewObjects_ReturnObjectOnTop()
        {
            _stack.Push("a");
            _stack.Push("b");

            var result = _stack.Pop();

            Assert.That(result, Is.EqualTo("b"));
        }

        [Test]
        public void Pop_StackWithAFewObjects_RemoveObjectOnTop()
        {
            _stack.Push("a");
            _stack.Push("b");

            _stack.Pop();

            Assert.That(_stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Peek_EmptyStack_ReturnInvalidOperationException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_StackWithAFewObjects_ReturnObjectOnTop()
        {
            _stack.Push("a");
            _stack.Push("b");

            var result = _stack.Peek();

            Assert.That(result, Is.EqualTo("b"));
        }
    }
}
