using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Dto
{
    public interface IHasTotalCount
    {
        long TotalCount { get; set; }
    }
}
