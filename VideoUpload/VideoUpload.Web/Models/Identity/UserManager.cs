using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Net.Mail;

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
            CustomEmailService = new CustomEmailService();            

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.Net Identity"));
            }      
        }
        public ICustomIdentityMessage CustomEmailService { get; set; }

        public async Task CustomSendEmailAsync(string userId, string subject, string body, string to, string credential)
        {
            if (CustomEmailService != null)
            {
                var identityMessage = new CustomIdentityMessage();
                var user = await GetEmailAsync(userId);
                identityMessage.Destination = user;
                identityMessage.Subject = subject;
                identityMessage.Body = body;
                identityMessage.To = to;
                identityMessage.Credential = credential;

                await CustomEmailService.SendAsync(identityMessage);                
            }
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

    public class CustomEmailService : ICustomIdentityMessage
    {        
        public async Task SendAsync(CustomIdentityMessage message)
        {
            var email = new MailMessage();
            email.From = new MailAddress(message.Destination);
            email.To.Add(message.To);
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;


            using (var client = new SmtpClient("192.168.5.10", 25))
            {
                client.Credentials = new System.Net.NetworkCredential(message.Destination, message.Credential);
                await client.SendMailAsync(email);
            }

        }
    }

    //inherit the IIdentityMessageService to overload the method SendAsync task
    public interface ICustomIdentityMessage
    {
        Task SendAsync(CustomIdentityMessage message);
    }

    //Make a derive class
    public class CustomIdentityMessage : IdentityMessage
    {
        public virtual string To { get; set; }
        public virtual string Credential { get; set; }
    }
}