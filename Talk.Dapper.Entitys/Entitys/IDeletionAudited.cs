using System;

namespace Talk.Dapper.Entitys.Entitys
{
    public interface IDeletionAudited
    {
        /// <summary>
        /// 是否被删除
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// 删除用户
        /// </summary>
        long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
