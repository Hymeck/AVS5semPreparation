using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace AVS5.Test
{
    public class StuffTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SequentialIListCompare()
        {
            IList<int> list1 = new List<int> {1, 2, 3};
            IList<int> list2 = new List<int> {3, 1, 2};

            var isEqual = list1.Count == list2.Count && 
                          list1.All(list2.Contains);
            Assert.AreEqual(true, isEqual);
        }

        [Test]
        public void PropertyTest()
        {
            var testClass = new SomeClass(4);
            Assert.AreEqual(false, testClass.IsZero);
            testClass.Value = 0;
            Assert.AreEqual(true, testClass.IsZero);
        }

        [Test]
        public void StackTest()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);
            // order:
            // 4
            // 3
            // 2
            // 1
            var top = stack.Pop();
            Assert.AreEqual(4, top);

            var listed = stack.ToImmutableList();
            Assert.AreEqual(3, listed[0]);
        }
        
        private class SomeClass
        {
            public int Value { get; set; }
            public bool IsZero => Value == 0;

            public SomeClass(int value) => Value = value;
        }
    }
}