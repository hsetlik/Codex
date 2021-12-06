using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentHistory
{
    public class DailyMetricBase<T>
    {
        public T Value { get; set; }
        public DateTime Date { get; set; }
    }
}