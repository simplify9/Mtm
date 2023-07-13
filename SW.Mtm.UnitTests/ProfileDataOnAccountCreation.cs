using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SW.Mtm.Domain;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace SW.Mtm.UnitTests
{
    [TestClass]
    public class ProfileDataInAccountCreation
    {

        [TestMethod]
        public void EmptyClaimOnAccountCreation()
        {
            //Create account
            var claims = new List<Claim>();
            Account account = new Account("testing", "testing@yopmail.com", "testing");
            if (account.ProfileData != null)
                claims.AddRange(account.ProfileData.Select(p => new Claim(p.Name, p.Value, p.Type)));
            Assert.IsTrue(claims.Count == 0);
        }
    }
}
