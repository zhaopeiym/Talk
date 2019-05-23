using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Dapper.Entitys.Entitys
{
    public interface ICreationAudited
    {
        /// <summary>
        ///创建人
        /// </summary>
        long CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
