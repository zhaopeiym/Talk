using System;
using Xunit;

namespace Talk.Cache.Tests
{

    public class Class1
    {

        [Fact]
        public void Test_string()
        {
            var time = DateTime.Now.AddMilliseconds(60) - DateTime.Now;
            EasyCache<string> str = new EasyCache<string>("k", time);
            str.AddData("aaa");
            Assert.Equal(str.GetData(), "aaa"); 
        }

        [Fact]
        public void Test_Obj()
        {
            var time = DateTime.Now.AddMilliseconds(60) - DateTime.Now;
            EasyCache<Order> obj = new EasyCache<Order>("k", time);
            obj.AddData(new Order() { Code = "123" });
            Assert.Equal(obj.GetData()?.Code, "123");
        }

        [Fact]
        public void Test_ExecuteNum()
        {
            var time = DateTime.Now.AddSeconds(5) - DateTime.Now;
            ExecuteNum e = new ExecuteNum("ind", time);
            //for (int i = 0; i < 50; i++)
            //{
            //    var num = e.GetNum();
            //}
            Assert.Equal(e.GetNum(), 1);
            Assert.Equal(e.GetNum(), 2);
            Assert.Equal(e.GetNum(), 3);
        }

    }
    public class Order
    {
        public string Code { get; set; }
    }
}
