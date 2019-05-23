namespace Talk.Dapper.Entitys.Entitys
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntity : IEntity<long>
    {
    }
}
