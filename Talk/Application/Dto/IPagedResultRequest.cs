using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Dto
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// 跳过多少条
        /// </summary>
        int SkipCount { get; set; }
    }
}
