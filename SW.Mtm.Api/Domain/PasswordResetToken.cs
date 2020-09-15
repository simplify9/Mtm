using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Domain
{
    internal class PasswordResetToken : BaseEntity<string>, IHasCreationTime
    {
        private PasswordResetToken()
        {
        }

        public PasswordResetToken(string accountId)
        {
            Id = Guid.NewGuid().ToString("N");
            AccountId = accountId;
        }

        public string AccountId { get; private set; }
        public DateTime CreatedOn { get ; set ; }
        
    }
}
