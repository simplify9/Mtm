
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Domain
{
    public class Invitation : BaseEntity<string>, IAudited, IDeletionAudited, IHasTenant
    {
        private Invitation()
        {
        }

        public static Invitation ByEmail(int tenantId, string email)
        {
            var invitation = New();
            invitation.Email = email ?? throw new ArgumentNullException(nameof(email));
            invitation.TenantId = tenantId;
            return invitation;
        }

        public static Invitation ByPhone(int tenantId, string phone)
        {
            var invitation = New();
            invitation.Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            invitation.TenantId = tenantId;
            return invitation;
        }

        public static Invitation ByAccountId(int tenantId, string accountId)
        {
            var invitation = New();
            invitation.AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            invitation.TenantId = tenantId;
            return invitation;
        }

        private static Invitation New()
        {
            return new Invitation
            {
                Id = Guid.NewGuid().ToString("N")
            };
        }

        public string AccountId { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public int TenantId { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public bool Deleted { get; private set; }
    }
}
