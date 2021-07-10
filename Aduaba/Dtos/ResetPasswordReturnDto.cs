using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class ResetPasswordReturnDto
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
