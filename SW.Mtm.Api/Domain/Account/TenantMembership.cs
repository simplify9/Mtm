using SW.Mtm.Model;
using System;

namespace SW.Mtm.Domain
{
    public class TenantMembership
    {
        public TenantMembership()
        {
        }

        public TenantMembership(int tenantId, MembershipType type)
        {
            TenantId = tenantId;
            Type = type;
        }

        public int TenantId { get; private set; }
        public MembershipType Type { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is TenantMembership membership &&
                   TenantId == membership.TenantId &&
                   Type == membership.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TenantId, Type);
        }
    }
}
