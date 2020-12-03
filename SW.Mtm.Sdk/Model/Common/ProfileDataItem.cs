using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Model
{
    public class ProfileDataItem : IEquatable<ProfileDataItem>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProfileDataItem);
        }

        public bool Equals(ProfileDataItem other)
        {
            return other != null &&
                   Name == other.Name &&
                   Value == other.Value &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value, Type);
        }

        public static bool operator ==(ProfileDataItem left, ProfileDataItem right)
        {
            return EqualityComparer<ProfileDataItem>.Default.Equals(left, right);
        }

        public static bool operator !=(ProfileDataItem left, ProfileDataItem right)
        {
            return !(left == right);
        }
    }
}
