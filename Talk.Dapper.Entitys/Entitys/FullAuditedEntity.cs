using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Dapper.Entitys.Entitys
{
    /// <summary>
    /// 全部（包含 创建、修改、删除）
    /// </summary>
    public abstract class FullAuditedEntity : DeletionAuditedEntity, IModificationAudited
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }
    }
}
