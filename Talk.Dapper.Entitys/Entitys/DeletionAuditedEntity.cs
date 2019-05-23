using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Dapper.Entitys.Entitys
{
    /// <summary>
    /// 包含（创建、删除）
    /// </summary>
    public abstract class DeletionAuditedEntity : CreationAuditedEntity, IDeletionAudited
    {
        /// <summary>
        /// 是否被删除
        /// </summary>
        public virtual bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 删除用户
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}
