using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Configuration;
using VideoUpload.Web.OoredooSOAP;
using System.Linq;

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
            OoredooMessageService = new OoredooSmsService();
            CustomEmailService = new CustomEmailService();
            

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.Net Identity"));
            }      
        }
        public IEmailIdentityMessage CustomEmailService { get; set; }        
        public IOoredooMessageService OoredooMessageService { get; set; }
        public string SmsStatusResult { get; set; }
        public async Task CustomSendEmailAsync(string userId, string subject, string body, string recipients, string emailPass)
        {
            if (CustomEmailService == null) throw new NotImplementedException("MessageService has not been implemented.");
            
            var identityMessage = new EmailIdentityMessage();
            var user = await GetEmailAsync(userId);
            identityMessage.Destination = user;
            identityMessage.Subject = subject;
            identityMessage.Body = body;
            identityMessage.To = recipients;
            identityMessage.Credential = emailPass;

            await CustomEmailService.SendAsync(identityMessage);
            
        }

        public async Task<string> OoredooSendSmsAsync(string mobile, string message)
        {
            if (OoredooMessageService == null) throw new NotImplementedException("MessageService has not been implemented.");
            
            var ooredooMessage = new OoredooMessage
            {
                Body = message,
                Destination = mobile
            };
            var result = await OoredooMessageService.SendAsync(ooredooMessage);
            return result;            
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
            id.AddClaim(new Claim("branchID", user.BranchID.ToString()));
            id.AddClaim(new Claim("jobtitle", user.JobTitle));
            id.AddClaim(new Claim("workaddress", user.WorkAddress));
            id.AddClaim(new Claim("phonenumber", user.PhoneNumber));
            id.AddClaim(new Claim("directline", user.DirectLine));
            id.AddClaim(new Claim("faxnumber", user.FaxNumber));
            id.AddClaim(new Claim("mobilenumber", user.MobileNumber));

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
        public virtual string Host { get; set; }
        public virtual string Port { get; set; }
    }
    public class CustomEmailService : IEmailIdentityMessage
    {
        public async Task SendAsync(EmailIdentityMessage message)
        {
            var email = new MailMessage();
            email.From = new MailAddress(message.Destination);

            //split the value semi-colon separated value
            string[] recipients = message.To.Split(';').Select(sValue => sValue.Trim()).ToArray();

            //loop and add to mailaddress
            for (int i = 0; i < recipients.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(recipients[i]))
                {
                    email.To.Add(new MailAddress(recipients[i]));
                }                
            }            
            
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;

            var host = ConfigurationManager.AppSettings["SmtpHost"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
           
            using (var client = new SmtpClient(host, port))
            {
                client.UseDefaultCredentials = true;                
                //client.Credentials = new System.Net.NetworkCredential(message.Destination.Trim(), message.Credential.Trim());               

                await client.SendMailAsync(email);
            }
        }
    }

    #endregion

    #region OoredooSmsService
    public interface IOoredooMessageService
    {
        Task<string> SendAsync(OoredooMessage message);
    }
    public class OoredooMessage
    {
        public virtual string Body { get; set; }
        public virtual string Destination { get; set; }
    }
    public class OoredooSmsService : IOoredooMessageService
    {
        public SmsStatus GetSmsStatus { get; private set; }

        public async Task<string> SendAsync(OoredooMessage message)
        {
            var customerID = Convert.ToInt32(ConfigurationManager.AppSettings["SmsCustomerID"]);
            var username = ConfigurationManager.AppSettings["SmsUsername"];
            var password = ConfigurationManager.AppSettings["SmsPassword"];

            // if you want to delay your message to a certain time
            // uncomment this and modify this based on your requirements and add to SendSmsAsync as an argument
            //string defDate = DateTime.UtcNow.AddHours(3).ToString("yyyyMMddhhmmss"); 

            MessengerSoapClient messenger = new MessengerSoapClient("MessengerSoap");

            SoapUser user = new SoapUser
            {
                CustomerID = customerID,
                Name = username,
                Password = password
            };
            AuthResult authData = messenger.Authenticate(user);

            if (authData.Result == "OK")
            {
                var sendResult =
                    await messenger.SendSmsAsync(
                    user,
                    authData.Originators[0],
                    message.Body,
                    message.Destination,
                    MessageType.Latin,
                    null, false, false, false);
                SmsStatus smsStatus = await messenger.GetSmsStatusAsync(user, sendResult.TransactionID, true);
                return smsStatus.Result;
            }
            return authData.Result;
        }
    }
    #endregion    
}