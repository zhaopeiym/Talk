﻿namespace Talk.Application.Dto
{
    public class ItemDto<T, T2, T3>
    {
        public ItemDto() { }
        public ItemDto(T key, T2 value, T3 value2)
        {
            Key = key;
            Value = value;
            Value2 = value2;
        }
        public T Key { get; set; }
        public T2 Value { get; set; }
        public T3 Value2 { get; set; }
    }

    public class ItemDto<T, T2>
    {
        public ItemDto() { }
        public ItemDto(T key, T2 value)
        {
            Key = key;
            Value = value;
        }
        public T Key { get; set; }
        public T2 Value { get; set; }
    }

    public class ItemDto<T>
    {
        public ItemDto() { }
        public ItemDto(T key, string value)
        {
            Key = key;
            Value = value;
        }
        public T Key { get; set; }
        public string Value { get; set; }
    }

    public class ItemDto
    {
        public ItemDto() { }
        public ItemDto(long key, string value)
        {
            Key = key;
            Value = value;
        }
        public long Key { get; set; }
        public string Value { get; set; }
    }
}
