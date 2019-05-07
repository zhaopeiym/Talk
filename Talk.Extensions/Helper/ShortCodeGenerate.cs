using System;
using System.Linq;

namespace Talk.Extensions.Helper
{
    public class ShortCodeGenerate
    {
        private string SeqKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqKey"></param>
        public ShortCodeGenerate(string seqKey)
        {
            //如：s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut
            SeqKey = seqKey;
        }

        /// <summary>
        /// 短码长度        
        /// </summary>
        private int CodeLength { get; } = 6;

        /// <summary>
        /// 注意：超过设定的长度可能会有异常数据
        /// </summary>
        private int MaxLength
        {
            get
            {
                return Convert("".PadLeft(CodeLength, SeqKey.Last())).ToString().Length - 1;
            }
        }

        /// <summary>
        /// 生成随机的0-9a-zA-Z字符串
        /// </summary>
        /// <returns></returns>
        private static string GenerateKeys()
        {
            string[] Chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
            int SeekSeek = unchecked((int)DateTime.Now.Ticks);
            Random SeekRand = new Random(SeekSeek);
            for (int i = 0; i < 100000; i++)
            {
                int r = SeekRand.Next(1, Chars.Length);
                string f = Chars[0];
                Chars[0] = Chars[r - 1];
                Chars[r - 1] = f;
            }
            return string.Join("", Chars);
        }

        /// <summary>
        /// 10进制转换为62进制【简单】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string Convert(long id)
        {
            if (id < 62)
            {
                return SeqKey[(int)id].ToString();
            }
            int y = (int)(id % 62);
            long x = id / 62;

            try
            {
                return Convert(x) + SeqKey[y];
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 将62进制转为10进制【简单】
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private long Convert(string num)
        {
            long v = 0;
            int Len = num.Length;
            for (int i = Len - 1; i >= 0; i--)
            {
                int t = SeqKey.IndexOf(num[i]);
                double s = (Len - i) - 1;
                long m = (long)(Math.Pow(62, s) * t);
                v += m;
            }
            return v;
        }

        /// <summary>
        /// 10进制转换为62进制【混淆】
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string ConfusionConvert(long num)
        {
            if (num.ToString().Length > MaxLength)
                throw new Exception($"转换值不能超过最大位数{MaxLength}");
            var n = num.ToString()
                   .PadLeft(MaxLength, '0')
                   .ToCharArray()
                   .Reverse();
            return Convert(long.Parse(string.Join("", n))).PadLeft(CodeLength, SeqKey.First());
        }

        /// <summary>
        /// 将62进制转为10进制【混淆】
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public long ConfusionConvert(string num)
        {
            if (num.Length > CodeLength)
                throw new Exception($"转换值不能超过最大位数{CodeLength }");
            var n = Convert(num).ToString().PadLeft(MaxLength, '0')
                .ToCharArray()
                .Reverse();
            return long.Parse(string.Join("", n));
        }
    }
}
