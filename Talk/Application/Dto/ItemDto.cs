namespace Talk.Application.Dto
{
    public class ItemDto<T, T2>
    {
        public T Key { get; set; }
        public T2 Value { get; set; }
    }

    public class ItemDto<T>
    {
        public T Key { get; set; }
        public string Value { get; set; }
    }

    public class ItemDto
    {
        public long Key { get; set; }
        public string Value { get; set; }
    }
}
