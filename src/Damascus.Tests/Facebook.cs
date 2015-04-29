using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;
using NUnit.Framework;

namespace Damascus.Tests
{
    [TestFixture]
    public class FacebookTest
    {
        private FacebookEventSender facebookSender;
        [SetUp]
        public void Init()
        {
            facebookSender = new FacebookEventSender()
            {
            };
        }

        [Test]
        public void TestPost()
        {
            facebookSender.CreateFacebookPost(new FacebookEventMessage
            {
                AccessToken = @"CAAVtczZBx3roBAO0xFcVFpke1UZCSeDJqsFOFZAcYWmwt7fKBAaQ7h58Kgi96g4ZCZBvt6jhDylrfwXRPNHcHBFFtrmIS6i829CKOZBfiXlZCkrzToqqLA3w9lyXwtzgBxzhZCBuyOsZBtyXw4QZBZBxeVbsYgLiEKwBJ4TT1XNovIXswuSAMw3osRZCxKmS4BqWr7Dr7ZASypZCiJwQUdIPngARRu",

                Invite = new ReducedInviteInput()
                {
                    Description = "My Test Invite",
                    Start = DateTime.Now,
                    Title = "Test unit invite"
                }

            });
        }
    }
}
