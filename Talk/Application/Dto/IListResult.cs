using System.Collections.Generic;

namespace Talk.Application.Dto
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
