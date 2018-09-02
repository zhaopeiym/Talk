using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Dto
{
    /// <summary>
    /// 分页
    /// </summary>
    public interface ILimitedResultRequest
    {
        /// <summary>
        /// 取多少条
        /// </summary>
        int MaxResultCount { get; set; }
    }
}
