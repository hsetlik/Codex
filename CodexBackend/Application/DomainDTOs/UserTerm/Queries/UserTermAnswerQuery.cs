using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
    public class UserTermAnswerQuery
    {
        public Guid UserTermId { get; set; }
        public int Answer { get; set; }
    }
}