using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;

namespace SW.Mtm.Domain
{
    public class TenantMembership : IEquatable<TenantMembership>
    {
        public TenantMembership()
        {
        }

        public TenantMembership(int tenantId, MembershipType type)
        {
            TenantId = tenantId;
            Type = type;
            ProfileData = new List<ProfileDataItem>();
        }

        public int TenantId { get; private set; }
        public MembershipType Type { get; private set; }
        public IEnumerable<ProfileDataItem> ProfileData { get; set; }

        public void ChangeType(MembershipType type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TenantMembership);
        }

        public bool Equals(TenantMembership other)
        {
            return other != null &&
                   CollectionComparer<ProfileDataItem>.Compare(ProfileData, other.ProfileData) &&
                   TenantId == other.TenantId &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TenantId, Type);
        }

        public static bool operator ==(TenantMembership left, TenantMembership right)
        {
            return EqualityComparer<TenantMembership>.Default.Equals(left, right);
        }

        public static bool operator !=(TenantMembership left, TenantMembership right)
        {
            return !(left == right);
        }
    }
}
