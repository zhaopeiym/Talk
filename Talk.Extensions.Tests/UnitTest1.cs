using NUnit.Framework;
using Talk.Extensions;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var a = NumberHelper.KBToShowSize(111);
            var b = NumberHelper.KBToShowSize(1024);
            var d = NumberHelper.KBToShowSize(1025);
            var c = NumberHelper.KBToShowSize(1025 * 1024);

            var a1 = NumberHelper.BToShowSize(111);
            var b1 = NumberHelper.BToShowSize(1024);
            var d1 = NumberHelper.BToShowSize(1025);
            var c1 = NumberHelper.BToShowSize(1025 * 1024);
        }
    }
}