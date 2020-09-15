using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW.Mtm.Model;
using System.Threading.Tasks;

namespace SW.Mtm.Sdk.UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        static TestServer server;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tcontext)
        {
            server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup>());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            server.Dispose();
        }


        [TestMethod]
        async public Task TestLoginWithEmailAndPassword()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var clientOptions = server.Host.Services.GetRequiredService<MtmClientOptions>();
            var loginRequest = new AccountLogin
            {
                Email = clientOptions.MockData["Email"],
                Password = clientOptions.MockData["Password"]
            };
            var result = await client.Login(loginRequest);

        }

        [TestMethod]
        public void TestLoginWithApiKey()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var loginRequest = new AccountLogin
            {

            };


        }

        [TestMethod]
        public void TestLoginWithPhone()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var loginRequest = new AccountLogin
            {

            };
        }


        [TestMethod]
        public void TestLoginWithOtp()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var loginRequest = new AccountLogin
            {

            };
        }

        [TestMethod]
        public void TestLoginWithRefereshToken()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var loginRequest = new AccountLogin
            {

            };
        }


        [TestMethod]
        async public Task TestChangePassword()
        {
            var client = server.Host.Services.GetRequiredService<IMtmClient>();
            var clientOptions = server.Host.Services.GetRequiredService<MtmClientOptions>();

            var loginRequest = new AccountLogin
            {
                Email = clientOptions.MockData["Email"],
                Password = clientOptions.MockData["Password"]
            };
            var loginResult = await client.Login(loginRequest);
            
            
            
            var accountId = clientOptions.MockData["AccountId"];
            var changePasswordRequest = new AccountChangePassword
            {
                CurrentPassword = clientOptions.MockData["Password"],
                NewPassword = "Mtm@1243",
            };
            await client.ChangePassword(changePasswordRequest);

            //Assert.AreEqual(changePasswordResult, null);





        }
    }
}
