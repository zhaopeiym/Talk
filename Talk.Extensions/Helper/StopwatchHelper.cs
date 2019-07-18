using System.Diagnostics;

namespace Talk.Extensions.Helper
{
    /// <summary>
    /// 执行时间性能监控
    /// </summary>
    public class StopwatchHelper
    {
        private Stopwatch stopwatch;
        public StopwatchHelper()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// 重新计算
        /// </summary>
        public void Restart()
        {
            stopwatch.Restart();
        }

        /// <summary>
        /// 执行总秒数
        /// </summary>
        /// <returns></returns>
        public double GetTotalSeconds()
        {
            var seconds = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            return seconds;
        }
    }
}
