using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Domain
{
    public class RefreshToken : BaseEntity<string>, IHasCreationTime
    {
        private RefreshToken()
        {
        }

        public RefreshToken(string accountId, LoginMethod loginMethod)
        {
            Id = Guid.NewGuid().ToString("N");
            LoginMethod = loginMethod;
            AccountId = accountId;
        }

        public string AccountId { get; private set; }
        public LoginMethod LoginMethod { get; set; }
        public DateTime CreatedOn { get ; set ; }
        
    }
}
