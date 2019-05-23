using System;

namespace Talk.Dapper.Entitys.Entitys
{
    public interface IModificationAudited
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        long? LastModifierUserId { get; set; }
    }
}
