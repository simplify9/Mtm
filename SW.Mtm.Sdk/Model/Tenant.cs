using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Model
{
    public class TenantAddAccount
    {
        public string AccountId { get; set; }
        public MembershipType MembershipType { get; set; }
    }

    public class TenantCreate
    {
        public string DisplayName { get; set; }
        public string OwnerEmail { get; set; }
        public EmailProvider OwnerEmailProvider { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerPassword { get; set; }
    }

    public class TenantCreateResult
    {
        public int TenantId { get; set; }
        public string AccountId { get; set; }
    }

    public class TenantInvite
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AccountId { get; set; }
    }

    public class TenantInviteResult
    {
        public string Id { get; set; }
    }

    public class TenantRemoveAccount
    {
        public string AccountId { get; set; }

    }
}
