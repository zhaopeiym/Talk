using NUnit.Framework;
using Talk.Extensions;
using Talk.Extensions.Helper;

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

        [Test]
        public void Test2()
        {
         
        }

        [Test]
        public void Test3()
        {
            var aaa = 123.5678.ToString("0.#");
            float f1 = 2.1f;
            float f2 = 6.105f;
            float f3 = 6.146f;
            float f4 = 6;

            var num1 = NumberHelper.KeepDigit(f1);
            var num2 = NumberHelper.KeepDigit(f2 );
            var num3 = NumberHelper.KeepDigit(f3);
            var num4 = NumberHelper.KeepDigit(f4);
            var num5 = NumberHelper.KeepDigit(num1 / num2, 1); 

            var num6 = NumberHelper.KeepDigit(NumberHelper.KeepDigit(f1 / 1000) / NumberHelper.KeepDigit(f2 / 1000), 1);
        }
    }
}