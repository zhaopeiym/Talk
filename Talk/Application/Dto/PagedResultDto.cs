using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Dto
{
    public class PagedResultDto<T> : IPagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public long TotalCount { get; set; }
        public PagedResultDto() { }
        public PagedResultDto(long totalCount, IReadOnlyList<T> items)       
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
