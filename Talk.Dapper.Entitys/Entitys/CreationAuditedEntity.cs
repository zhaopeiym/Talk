using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Dapper.Entitys.Entitys
{
    /// <summary>
    /// 包含（创建）
    /// </summary>
    public abstract class CreationAuditedEntity : EntityBase, ICreationAudited
    {
        /// <summary>
        ///创建人
        /// </summary>
        public virtual long CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
