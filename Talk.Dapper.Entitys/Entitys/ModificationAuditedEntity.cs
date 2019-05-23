using System;

namespace Talk.Dapper.Entitys.Entitys
{
    /// <summary>
    /// 包含（创建、修改）
    /// </summary>
    public abstract class ModificationAuditedEntity : CreationAuditedEntity, IModificationAudited
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
