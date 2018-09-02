using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Dto
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}
