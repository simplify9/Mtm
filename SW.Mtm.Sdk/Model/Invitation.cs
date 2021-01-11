using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Model
{
    public class InvitationAccept
    {
    }

    public class InvitationSearch
    {
        public string Email { get; set; }
    }

    public class InvitationGet : InvitationSearchResult { }

    public class InvitationSearchResult
    {
        public string Id { get; set; }
        public string AccountId { get;  set; }
        public string Email { get;  set; }
        public string Phone { get;  set; }
        public int TenantId { get;  set; }
        public string CreatedBy { get;  set; }
        public DateTime CreatedOn { get;  set; }
        public string ModifiedBy { get;  set; }
        public DateTime? ModifiedOn { get;  set; }
    }

    public class InvitationCancel
    {
        public string Email { get; set; }
    }

    //public class InvitationAcceptResult
    //{
    //    public string AccountId { get; set; }
    //    public string MyProperty { get; set; }
    //}
}
