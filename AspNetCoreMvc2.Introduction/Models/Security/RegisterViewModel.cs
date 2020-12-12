using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvc2.Introduction.Models.Security
{
    public class RegisterViewModel // Sisteme ilk defa girecek olan kullanıcının kayıt işlemi için kullanılacak.
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string ConfirmedPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
