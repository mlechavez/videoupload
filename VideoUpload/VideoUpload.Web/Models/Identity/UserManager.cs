using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VideoUpload.Web.Models.Identity
{
    public class UserManager : UserManager<IdentityUser, string>
    {
        public UserManager(IUserStore<IdentityUser, string> store) : base(store)
        {
            UserValidator = new UserValidator<IdentityUser, string>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            PasswordValidator = new PasswordValidator
            {
                RequireUppercase = true,
                RequireLowercase = true,
                RequiredLength = 6
            };
            ClaimsIdentityFactory = new AppClaimsIdentityFactory();
        }
    }    
    public class AppClaimsIdentityFactory : ClaimsIdentityFactory<IdentityUser, string>
    {
        public async override Task<ClaimsIdentity> CreateAsync(UserManager<IdentityUser, string> manager, IdentityUser user, string authenticationType)
        {
            var id = await base.CreateAsync(manager, user, authenticationType);

            id.AddClaim(new Claim("firstname", user.FirstName));
            id.AddClaim(new Claim("lastname", user.LastName));
            id.AddClaim(new Claim("email", user.Email));
            id.AddClaim(new Claim("emailpass", user.EmailPass));

            return id;
        }
    }
}