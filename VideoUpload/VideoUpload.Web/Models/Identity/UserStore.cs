using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoUpload.Core;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using System.Security.Claims;

namespace VideoUpload.Web.Models.Identity
{
    public class UserStore : IUserStore<IdentityUser, string>, IUserClaimStore<IdentityUser, string>,
        IUserEmailStore<IdentityUser, string>, IUserPasswordStore<IdentityUser, string>, 
        IUserSecurityStampStore<IdentityUser, string>, IQueryableUserStore<IdentityUser, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IQueryable<IdentityUser> Users
        {
            get
            {
                return _unitOfWork.Users.GetAll()
                        .Select(x => GetIdentityUser(x))
                        .AsQueryable();
            }
        }

        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private User GetUser(IdentityUser identityUser)
        {
            if (identityUser == null) return null;

            var user = new User();
            populateUser(user, identityUser);
            return user;            
        }

        private void populateUser(User user, IdentityUser identityUser)
        {
            user.UserID = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.FirstName = identityUser.FirstName;
            user.LastName = identityUser.LastName;
            user.JobTitle = identityUser.JobTitle;
            user.EmployeeNo = identityUser.EmployeeNo;            
            user.Email = identityUser.Email;
            user.EmailPass = identityUser.EmailPass;
            user.EmailConfirmed = identityUser.EmailConfirmed;
            user.IsActive = identityUser.IsActive;
        }

        private IdentityUser GetIdentityUser(User user)
        {
            if (user == null) return null;
            var identityUser = new IdentityUser();
            populateIdentityUser(identityUser, user);
            return identityUser;
        }

        private void populateIdentityUser(IdentityUser identityUser, User user)
        {
            identityUser.Id = user.UserID;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
            identityUser.FirstName = user.FirstName;
            identityUser.LastName = user.LastName;
            identityUser.JobTitle = user.JobTitle;
            identityUser.EmployeeNo = user.EmployeeNo;
            identityUser.Email = user.Email;
            identityUser.EmailPass = user.EmailPass;
            identityUser.EmailConfirmed = user.EmailConfirmed;
            identityUser.IsActive = user.IsActive;
        }
        #region IUserStore      
        public Task CreateAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            var u = GetUser(user);
            _unitOfWork.Users.Add(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            var u = _unitOfWork.Users.GetById(user.Id);
            if (u == null) throw new ArgumentException("IdentityUser does not correspond to a user entity", "user");
            _unitOfWork.Users.Remove(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            //let Unity mvc do the disposing
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            return Task.FromResult(GetIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.Users.GetByUsername(userName);
            return Task.FromResult(GetIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null ) throw new ArgumentNullException("user");

            var u = _unitOfWork.Users.GetById(user.Id);

            if (u == null) throw new ArgumentException("IdentityUser does not correspond to a user entity", "user");

            populateUser(u, user);
            _unitOfWork.Users.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IUserEmailStore        
        public Task SetEmailAsync(IdentityUser user, string email)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {            
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
            var user = _unitOfWork.Users.GetByEmail(email);
            return Task.FromResult(GetIdentityUser(user));
        }
        #endregion
        #region IUserPasswordStore
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }
        #endregion
        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion
        #region IUserClaimStore
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            var u = _unitOfWork.Users.GetById(user.Id);
            IList<Claim> claims = u.UserClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();
            return Task.FromResult(claims);
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (claim == null) throw new ArgumentNullException("claim");

            var u = _unitOfWork.Users.GetById(user.Id);

            if (u == null) throw new ArgumentException("IdentityUser does not correspond to a user entity", "user");

            var c = new UserClaim { ClaimType = claim.Type, ClaimValue = claim.Value, User = u };
            u.UserClaims.Add(c);
            _unitOfWork.Users.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (claim == null) throw new ArgumentNullException("claim");

            var u = _unitOfWork.Users.GetById(user.Id);

            if (u == null) throw new ArgumentException("IdentityUser does not correspond to a user entity", "user");
            var c = u.UserClaims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.UserClaims.Remove(c);
            _unitOfWork.Users.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}