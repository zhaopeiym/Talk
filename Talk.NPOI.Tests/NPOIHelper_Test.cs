using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Xunit;

namespace Talk.NPOI.Tests
{
    public class NPOIHelper_Test
    {
        private string basePath = "";
        public NPOIHelper_Test()
        {
            DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
            basePath = dir.Parent.Parent.Parent.FullName + @"\Data\";//父目录         
        }

        [Fact]
        public void xlsx读取xls写入测试()
        {
            var table = NPOIHelper.GetDataTables(basePath + "空运模板1.xlsx");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\空运模板1_Test.xls");
        }

        [Fact]
        public void xls读取xlsx写入测试()
        {
            var table = NPOIHelper.GetDataTables(basePath + "空运模板2.xls");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\空运模板2_Test.xlsx");
        }

        [Fact]
        public void Excel读到DataTable再转换成实体()
        {
            //读取成DataTable
            var tableList = NPOIHelper.GetDataTables(basePath + "空运模板2.xls");
            //DataTable转成实体
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*起运空港"]?.ToString();
                air.ToTerminal = row["*目的空港"]?.ToString(); ;
                air.Carrier = row["*航空公司"]?.ToString(); ;
                //air.ETD = row["*开航日期"]?.ToDateTimeOrNull();
            });
        }

        [Fact]
        public void 测试实体转换成DataTable()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "空运模板2.xls");
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*起运空港"].ToString();
                air.ToTerminal = row["*目的空港"].ToString();
                air.Carrier = row["*航空公司"].ToString();
            });

            Dictionary<string, string> head = new Dictionary<string, string>()
            {
                { "FromTerminal","*起运空港"},
                { "ToTerminal","*目的空港"},
                { "Carrier","*航空公司"},
            };
            //实体转成DataTable
            var table = listEntity.ToDataTable(head, (row, air) =>
            {
                row[head["FromTerminal"]] = air.FromTerminal;
                row[head["ToTerminal"]] = air.ToTerminal;
                row[head["Carrier"]] = air.Carrier;
            });
        }

        [Fact]
        public void 测试实体转换成DataTable2()
        {
            var list = new List<AirEntity>();
            list.Add(new AirEntity()
            {
                Carrier = "Carrier",
                ETD = DateTime.Now,
                FromTerminal = "FromTerminal",
                ToTerminal = "ToTerminal",
            });
            list.Add(new AirEntity()
            {
                Carrier = "Carrier2",
                ETD = DateTime.Now,
                FromTerminal = "FromTerminal2",
                ToTerminal = "ToTerminal2",
            });


            var table = list.ToDatatableFromList(false);
            table.DataTableToExcel(@"D:\123333.xls");
            //var table = list.ToExcel();
            //list.ToExcel(@"D:\123.xls");
        }

        [Fact]
        public void 多个Shett读取成多个Table再分别写入Excel()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "空运模板2.xls");
            var i = 0;
            foreach (var table in tableList)
            {
                if (table != null)
                    NPOIHelper.DataTableToExcel(table, basePath + @"Temp\空运模板2" + i++ + ".xlsx");
            }
        }

        [Fact]
        public void 获取自定义表头和内容开始行()
        {
            var list = new List<Point>() {
                 new Point(1,0),
                 new Point(2,1),
                 new Point(2,2),
                 new Point(1,3),
                 new Point(1,4),
                 new Point(1,5),
                 new Point(1,6),
            };
            var table = NPOIHelper.GetDataTable(basePath + "空运模板2.xls", list, 3, 1);

            NPOIHelper.DataTableToExcel(table, basePath + @"Temp\空运模板2.xlsx");
        }

        [Fact]
        public void 修改源Excel文件()
        {
            var filePath = basePath + @"Temp\赛嘉全航线_Copy.xlsx";
            var dic = new Dictionary<int, string>() {
                { 4,"备注aaaa"},
                { 5,"备注a士大夫aaa"},
                { 6,"werewolf"},
                { 7,"士大我惹我夫"},
                { 8,"委任為"},
                { 10,"斯蒂芬斯蒂芬第三方斯蒂芬企鹅温热万人vqwreqwerewqvbewreyvrtybw"},
            };

            NPOIHelper.SaveExcel(basePath + "空运模板2.xls", filePath, 1, 14, dic);
        }

        [Fact]
        public void Excel转实体集合()
        {
            var entitys = NPOIHelper.ToEntitys<AirEntity>(basePath + "空运模板2.xls");
        }

        public class AirEntity
        {
            //[Alias("*起运空港")]
            public string FromTerminal { get; set; }
            [Alias("*目标空港")]
            public string ToTerminal { get; set; }
            [Alias("*航空公司")]
            public string Carrier { get; set; }
            [Alias("*开航日期")]
            public DateTime? ETD { get; set; }
        }
    }
}
