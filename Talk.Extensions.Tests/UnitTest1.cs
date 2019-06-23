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
            //var aa =  (12.43 * 10) % 10;
            var temp2 = WetBulbTemperatureHelper.Calculate(33, 19.1f);
            var temp = WetBulbTemperatureHelper.Calculate(25, 18.8f);
            var num1 = WetBulbTemperatureHelper.Calculate(20, 18);
            var num2 = WetBulbTemperatureHelper.Calculate(21, 18);
            var num3 = WetBulbTemperatureHelper.Calculate(20, 18.1f);
            var num4 = WetBulbTemperatureHelper.Calculate(21, 18.1f);
            var num5 = WetBulbTemperatureHelper.Calculate(20, -3);
        }

        [Test]
        public void Test3()
        {
            var aaa = 123.5678.ToString("0.#");
            float f1 = 27.8f;
            float f2 = 6.1f;

            var num1 = NumberHelper.KeepDigit(f1 / 1000);
            var num2 = NumberHelper.KeepDigit(f2 / 1000);
            var num3 = NumberHelper.KeepDigit(num1 / num2, 1); 

            var num4 = NumberHelper.KeepDigit(NumberHelper.KeepDigit(f1 / 1000) / NumberHelper.KeepDigit(f2 / 1000), 1);
        }
    }
}