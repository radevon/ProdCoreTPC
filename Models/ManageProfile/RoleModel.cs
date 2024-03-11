using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Models
{
    public class RoleModel
    {
        
        [HiddenInput(DisplayValue =false)]
        public string Id { get; set; }

        [Display(Name="Название")]
        [Required(ErrorMessage ="Не заполнено поле \"Название\"")]
        public string Name { get; set; }
    }
}
