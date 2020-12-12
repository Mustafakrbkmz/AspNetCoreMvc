using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvc2.Introduction.Models.Security
{
    public class ResetPasswordViewModel
    {

        public string Code { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }  //Yeni şifresini girmesini istiyoruz.
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
    }
}
