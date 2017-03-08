using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using Twilio.Clients;
using System.Configuration;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
            CustomSmsService = new CustomSmsService();        

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.Net Identity"));
            }      
        }
        public IEmailIdentityMessage CustomEmailService { get; set; }
        public ISmsIdentityMessage CustomSmsService { get; set; }

        public async Task CustomSendEmailAsync(string userId, string subject, string body, string to, string credential)
        {
            if (CustomEmailService != null)
            {
                var identityMessage = new EmailIdentityMessage();
                var user = await GetEmailAsync(userId);
                identityMessage.Destination = user;
                identityMessage.Subject = subject;
                identityMessage.Body = body;
                identityMessage.To = to;
                identityMessage.Credential = credential;

                await CustomEmailService.SendAsync(identityMessage);                
            }
        }

        public async Task CustomSendSmsAsync(string userId, string to, string body)
        {
            if (CustomSmsService != null)
            {
                var identityMesaage = new SmsIdentityMessage();
                identityMesaage.To = to;
                //identityMesaage.Destination = from;
                identityMesaage.Body = body;

                await CustomSmsService.SendAsync(identityMesaage);
            }
        }
    }

    #region ClaimsIdentityFactory
    public class AppClaimsIdentityFactory : ClaimsIdentityFactory<IdentityUser, string>
    {
        public async override Task<ClaimsIdentity> CreateAsync(UserManager<IdentityUser, string> manager, IdentityUser user, string authenticationType)
        {
            var id = await base.CreateAsync(manager, user, authenticationType);

            id.AddClaim(new Claim("firstname", user.FirstName));
            id.AddClaim(new Claim("lastname", user.LastName));
            id.AddClaim(new Claim("email", user.Email));
            id.AddClaim(new Claim("emailpass", user.EmailPass));

            var claims = await manager.GetClaimsAsync(user.Id);
                        
            foreach (var claim in claims)
            {
                id.AddClaim(claim);
            }

            return id;
        }
    }
    #endregion

    #region EmailService
    public interface IEmailIdentityMessage
    {
        Task SendAsync(EmailIdentityMessage message);
    }
    public class EmailIdentityMessage : IdentityMessage
    {
        public virtual string To { get; set; }
        public virtual string Credential { get; set; }
    }
    public class CustomEmailService : IEmailIdentityMessage
    {
        public async Task SendAsync(EmailIdentityMessage message)
        {
            var email = new MailMessage();
            //email.From = new MailAddress(message.Destination);
            email.From = new MailAddress("kyocera.km3060@boraq-porsche.com.qa");
            email.To.Add(message.To);
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;

            using (var client = new SmtpClient("78.100.48.220", 25))
            {
                client.Credentials = new System.Net.NetworkCredential("kyocera.km3060@boraq-porsche.com.qa", "kyocera123");
                await client.SendMailAsync(email);
            }
        }
    }
   
    #endregion

    #region SmsService
    public interface ISmsIdentityMessage
    {
        Task SendAsync(SmsIdentityMessage message);
    }
    public class SmsIdentityMessage : IdentityMessage
    {
        public virtual string To { get; set; }
        public virtual string Credential { get; set; }
    }
    public class CustomSmsService : ISmsIdentityMessage
    {
        public async Task SendAsync(SmsIdentityMessage message)
        {
            var accountID = ConfigurationManager.AppSettings["SMSAccountIdentification"];
            var authToken = ConfigurationManager.AppSettings["SMSAccountPassword"];
            var myNumber = ConfigurationManager.AppSettings["SMSAccountFrom"];

            TwilioClient.Init(accountID, authToken);

            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(message.To),
                from: new PhoneNumber(myNumber),
                body: message.Body);

            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Sid);
            //Twilio doesn't currently have an async API, so return success.            
            //Twilio End
        }
    }
    #endregion
}