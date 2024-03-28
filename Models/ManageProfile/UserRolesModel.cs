using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Models
{
    // модель для редактирования ролей пользователя
    public class UserRolesModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public Dictionary<string,bool> Roles { get; set; }
    }
}
