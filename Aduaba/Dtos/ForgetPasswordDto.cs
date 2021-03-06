using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class ForgetPasswordDto
    {
        [Required]
        public string Email { get; set; }
        public string  Errors { get; set; }
        public string Token { get; set; }
        public string RedirectUri { get; set; }
    }
}
