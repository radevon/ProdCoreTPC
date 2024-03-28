using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Models
{
    public class PasswordResetModel
    {

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }
        [HiddenInput]
        public string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string PasswordResetToken { get; set; }

        [Required(ErrorMessage ="Поле \"Новый пароль\" обязательно должно быть заполнено!")]
        [DataType(DataType.Password)]
        [Display(Name ="Новый пароль")]
        public string NewPassword { get; set; }
    }
}
