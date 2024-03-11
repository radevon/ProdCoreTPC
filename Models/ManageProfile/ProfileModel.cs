using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProdCoreTPC.Models
{
    public class ProfileModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [Required(ErrorMessage ="Имя пользователя не заполнено")]
        [Display(Name = "Имя пользователя / Логин")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Придумайте надежный пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Display(Name = "Пароль повторный")]
        [DataType(DataType.Password)]
        [Compare(otherProperty:"Password",ErrorMessage ="Пароли не совпадают")]
        public string PasswordRepeat { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
    }
}
