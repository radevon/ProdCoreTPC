using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProdCoreTPC
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Не заполнено  поле \"Логин\"")]
        [Display(Name="Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не заполнено поле \"Пароль\"")]
        [Display(Name = "Пароль пользователя")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
