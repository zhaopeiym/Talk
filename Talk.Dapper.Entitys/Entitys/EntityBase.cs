using Dapper.Contrib.Extensions;

namespace Talk.Dapper.Entitys.Entitys
{
    public abstract class EntityBase : IEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
