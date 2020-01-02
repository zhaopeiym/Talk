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
            basePath = dir.Parent.Parent.Parent.FullName + @"\Data\";//��Ŀ¼         
        }

        [Fact]
        public void xlsx��ȡxlsд�����()
        {
            var table = NPOIHelper.GetDataTables(basePath + "����ģ��1.xlsx");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\����ģ��1_Test.xls");
        }

        [Fact]
        public void xls��ȡxlsxд�����()
        {
            var table = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\����ģ��2_Test.xlsx");
        }

        [Fact]
        public void Excel����DataTable��ת����ʵ��()
        {
            //��ȡ��DataTable
            var tableList = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");
            //DataTableת��ʵ��
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*���˿ո�"]?.ToString();
                air.ToTerminal = row["*Ŀ�Ŀո�"]?.ToString(); ;
                air.Carrier = row["*���չ�˾"]?.ToString(); ;
                //air.ETD = row["*��������"]?.ToDateTimeOrNull();
            });
        }

        [Fact]
        public void ����ʵ��ת����DataTable()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*���˿ո�"].ToString();
                air.ToTerminal = row["*Ŀ�Ŀո�"].ToString();
                air.Carrier = row["*���չ�˾"].ToString();
            });

            Dictionary<string, string> head = new Dictionary<string, string>()
            {
                { "FromTerminal","*���˿ո�"},
                { "ToTerminal","*Ŀ�Ŀո�"},
                { "Carrier","*���չ�˾"},
            };
            //ʵ��ת��DataTable
            var table = listEntity.ToDataTable(head, (row, air) =>
            {
                row[head["FromTerminal"]] = air.FromTerminal;
                row[head["ToTerminal"]] = air.ToTerminal;
                row[head["Carrier"]] = air.Carrier;
            });
        }

        [Fact]
        public void ����ʵ��ת����DataTable2()
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
        public void ���Shett��ȡ�ɶ��Table�ٷֱ�д��Excel()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");
            var i = 0;
            foreach (var table in tableList)
            {
                if (table != null)
                    NPOIHelper.DataTableToExcel(table, basePath + @"Temp\����ģ��2" + i++ + ".xlsx");
            }
        }

        [Fact]
        public void ��ȡ�Զ����ͷ�����ݿ�ʼ��()
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
            var table = NPOIHelper.GetDataTable(basePath + "����ģ��2.xls", list, 3, 1);

            NPOIHelper.DataTableToExcel(table, basePath + @"Temp\����ģ��2.xlsx");
        }

        [Fact]
        public void �޸�ԴExcel�ļ�()
        {
            var filePath = basePath + @"Temp\����ȫ����_Copy.xlsx";
            var dic = new Dictionary<int, string>() {
                { 4,"��עaaaa"},
                { 5,"��עaʿ���aaa"},
                { 6,"werewolf"},
                { 7,"ʿ�������ҷ�"},
                { 8,"ί�Ξ�"},
                { 10,"˹�ٷ�˹�ٷҵ�����˹�ٷ������������vqwreqwerewqvbewreyvrtybw"},
            };

            NPOIHelper.SaveExcel(basePath + "����ģ��2.xls", filePath, 1, 14, dic);
        }

        [Fact]
        public void Excelתʵ�弯��()
        {
            var entitys = NPOIHelper.ToEntitys<AirEntity>(basePath + "����ģ��2.xls");
        }

        public class AirEntity
        {
            //[Alias("*���˿ո�")]
            public string FromTerminal { get; set; }
            [Alias("*Ŀ��ո�")]
            public string ToTerminal { get; set; }
            [Alias("*���չ�˾")]
            public string Carrier { get; set; }
            [Alias("*��������")]
            public DateTime? ETD { get; set; }
        }
    }
}
