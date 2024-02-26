using Microsoft.AspNetCore.Identity;
using ProdCoreTPC.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Code.Repositories
{
    public class AppRoleRepository : IRepository<IdentityRole, String>
    {
        private RoleManager<IdentityRole> _roleManager;

        public AppRoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // создаю пользователя без пароля
        public bool Add(IdentityRole item)
        {
            Task<IdentityResult> result = _roleManager.CreateAsync(item);
            result.Wait();
            return result.Result.Succeeded;
        }

        public bool Delete(string id)
        {
            IdentityRole role = this.Get(id);
            if (role != null)
            {
                Task<IdentityResult> result = _roleManager.DeleteAsync(role);
                result.Wait();
                return result.Result.Succeeded;
            }
            else
                return false;
            
        }

        public IEnumerable<IdentityRole> Find(Func<IdentityRole, bool> predicate)
        {
            return _roleManager.Roles.Where(predicate).ToList();
        }

        public IdentityRole Get(string id)
        {
            return _roleManager.Roles.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return _roleManager.Roles.ToList();
        }

        public bool Update(IdentityRole item)
        {
            Task<IdentityResult> result= _roleManager.UpdateAsync(item);
            result.Wait();
            return result.Result.Succeeded;
        }
    }
}
