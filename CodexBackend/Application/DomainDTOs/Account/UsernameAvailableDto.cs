using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Account
{
    public class UsernameAvailableDto
    {
        public string Username { get; set; }
        public bool IsAvailable { get; set; }
    }
}