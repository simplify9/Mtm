using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;

namespace SW.Mtm.Domain
{
    public class Tenant : BaseEntity, IAudited, IDeletionAudited
    {

        private Tenant()
        {
        }

        public Tenant(string name)
        {
            DisplayName = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string DisplayName { get; set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public bool Deleted { get; private set; }
    }
}
