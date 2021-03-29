using Microsoft.EntityFrameworkCore.Internal;
using SW.EfCoreExtensions;
using SW.HttpExtensions;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SW.Mtm.Domain
{
    public class Account : BaseEntity<string>, IAudited, IDeletionAudited, IHasOptionalTenant
    {
        public const string SystemId = "1";


        private Account()
        {
        }

        public Account(string displayName, ApiCredential apiCredential) : this(displayName, LoginMethod.ApiKey, OtpType.None)
        {
            _ApiCredentials.Add(apiCredential);
        }

        public Account(string displayName, string phone) : this(displayName, LoginMethod.PhoneAndOtp, OtpType.None)
        {
            Phone = phone;
            PhoneVerified = true;
        }

        public Account(string displayName, string email, string password) : this(displayName, email, EmailProvider.None)
        {
            Password = password;
        }
        public Account(string displayName, string email, string password, OtpType secondFactorMethod) : this(displayName, email, EmailProvider.None, secondFactorMethod)
        {
            Password = password;
        }
        public Account(string displayName, string email, EmailProvider accountProvider) : this(displayName, LoginMethod.EmailAndPassword,OtpType.None)
        {
            Email = email;
            EmailProvider = accountProvider;
        }
        public Account(string displayName, string email, EmailProvider accountProvider, OtpType secondFactorMethod) : this(displayName, LoginMethod.EmailAndPassword, secondFactorMethod)
        {
            Email = email;
            EmailProvider = accountProvider;
        }

        private Account(string displayName, LoginMethod loginMethods, OtpType secondFactorMethod)
        {
            Id = Guid.NewGuid().ToString("N");
            SecondFactorMethod = secondFactorMethod;
            LoginMethods = loginMethods;
            DisplayName = displayName;
            _ApiCredentials = new HashSet<ApiCredential>();
            _TenantMemberships = new HashSet<TenantMembership>();
            ProfileData = new ProfileDataItem[] { };
            Roles = new string[] { };
        }

        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string DisplayName { get; set; }

        public EmailProvider EmailProvider { get; private set; }
        public LoginMethod LoginMethods { get; private set; }
        public OtpType SecondFactorMethod { get; private set; }
        public string SecondFactorKey { get; private set; }
        public bool IsSecondFactorKeyVerified { get; private set; }


        public void VerifySecondFactorKey()
        {
            IsSecondFactorKeyVerified = true;
        }
        public void SetupSecondFactor(OtpType otpType, string key)
        {
            SecondFactorMethod = otpType;
            if (otpType == OtpType.Totp)
            {
                SecondFactorKey = key ?? throw new ArgumentNullException();
            }
            else
            {
                SecondFactorKey = null;
            }
        }


        public string Password { get; private set; }
        public void SetPassword(string password)
        {
            Password = password;
        }

        public bool Landlord { get; private set; }
        public void SetLandlord(bool landlord)
        {
            Landlord = landlord;
        }

        public bool EmailVerified { get; private set; }
        public void SetEmailVerified(bool emailVerified)
        {
            EmailVerified = emailVerified;
        }

        public bool PhoneVerified { get; private set; }
        public void SetPhoneVerified(bool phoneVerified)
        {
            PhoneVerified = phoneVerified;
        }

        readonly HashSet<ApiCredential> _ApiCredentials;
        public IReadOnlyCollection<ApiCredential> ApiCredentials => _ApiCredentials;
        public void SetApiCredentials(IEnumerable<ApiCredential> apiCredentials)
        {
            _ApiCredentials.Update(apiCredentials);
        }

        public IEnumerable<ProfileDataItem> ProfileData { get; set; }

        public int? TenantId { get; private set; }

        readonly HashSet<TenantMembership> _TenantMemberships;
        public IReadOnlyCollection<TenantMembership> TenantMemberships => _TenantMemberships;
        public bool SetTenantId(int tenantId)
        {
            if (_TenantMemberships.Any(t => t.TenantId == tenantId))
            {
                TenantId = tenantId;
                return true;
            }

            return false;

        }


        //public void SetTenantMembership(IEnumerable<TenantMembership> tenantMemberships)
        //{
        //    _TenantMemberships.Update(tenantMemberships);

        //    if (TenantId != null && !_TenantMemberships.Any(t => t.TenantId == TenantId))
        //        TenantId = null;
        //}

        public bool AddTenantMembership(TenantMembership membership)
        {
            if (!_TenantMemberships.Any(t => t.TenantId == membership.TenantId))
            {
                if (_TenantMemberships.Count == 0)
                    if (TenantId == null) TenantId = membership.TenantId;

                return _TenantMemberships.Add(membership);
            }
            return false;
        }

        public bool RemoveTenantMembership(int tenantId)
        {
            var membership = _TenantMemberships.Where(t => t.TenantId == tenantId).SingleOrDefault();
            if (membership != null)
            {
                if (TenantId == tenantId) TenantId = null;
                return _TenantMemberships.Remove(membership);
            }
            return false;
        }

        public bool UpdateTenantMembership(int tenantId, MembershipType membershipType)
        {
            var oldMembership = _TenantMemberships.Where(t => t.TenantId == tenantId && t.Type != membershipType).SingleOrDefault();
            if (oldMembership != null && _TenantMemberships.Remove(oldMembership))
            {
                return _TenantMemberships.Add(new TenantMembership(tenantId, membershipType));
            }
            return false;
        }
        
        public bool AddEmailLoginMethod(string email,string password)
        {
            Password = password;
            return AddEmailLoginMethod(email, EmailProvider.None);
        }
        public bool AddEmailLoginMethod(string email,EmailProvider provider)
        {
            Email = email;
            EmailProvider = provider;
            return AddLoginMethod(LoginMethod.EmailAndPassword);
        }

        public bool AddPhoneLoginMethod(string phone)
        {
            Phone = phone;
            PhoneVerified = true;
            return AddLoginMethod(LoginMethod.PhoneAndOtp);
        }
        public bool AddApiKeyLoginMethod(ApiCredential apiCredential)
        {
            _ApiCredentials.Add(apiCredential);
            return AddLoginMethod(LoginMethod.ApiKey);
        }
        private bool AddLoginMethod(LoginMethod loginMethod)
        {
            if ((LoginMethods & loginMethod) == loginMethod)
                return false;
            LoginMethods &= loginMethod;
            return true;
        }
        private bool RemoveLoginMethod(LoginMethod loginMethod)
        {
            if ((LoginMethods & loginMethod) != loginMethod)
                return false;
            LoginMethods |= loginMethod;
            return true;
        }

        public string[] Roles { get; private set; }
        public bool Disabled { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public bool Deleted { get; private set; }
    }
}
