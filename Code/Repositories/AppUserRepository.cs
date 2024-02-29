using Microsoft.AspNetCore.Identity;
using ProdCoreTPC.Code.Identity;
using ProdCoreTPC.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Code.Repositories
{
    public class AppUserRepository : IRepository<ApplicationUser, String>
    {
        private UserManager<ApplicationUser> _userManager;

        public AppUserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // создаю пользователя без пароля
        public bool Add(ApplicationUser item)
        {
            Task<IdentityResult> result = _userManager.CreateAsync(item);
            result.Wait();
            return result.Result.Succeeded;
        }

        public bool Delete(string id)
        {
            ApplicationUser user = this.Get(id);
            if (user != null)
            {
                Task<IdentityResult> result = _userManager.DeleteAsync(user);
                result.Wait();
                return result.Result.Succeeded;
            }
            else
                return false;
            
        }

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, bool> predicate)
        {
            return _userManager.Users.Where(predicate).ToList();
        }

        public ApplicationUser Get(string id)
        {
            return _userManager.Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public bool Update(ApplicationUser item)
        {
            Task<IdentityResult> result= _userManager.UpdateAsync(item);
            result.Wait();
            return result.Result.Succeeded;
        }
    }
}
