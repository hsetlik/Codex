using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,10}$", ErrorMessage = "Password rules not met")]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string NativeLanguage { get; set; }

        [Required]
        public string StudyLanguage { get; set; }
    }
}