using System.Collections.Generic;
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
    }
}