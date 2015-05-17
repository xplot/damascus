using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Message;
using Damascus.Message.Command;
using Facebook;

namespace Damascus.Core
{
    public class FacebookEventSender : IFacebookEventSender
    {
        public void CreateFacebookEvent(FacebookEventMessage facebookEventMessage)
        {
            var facebookClient = new FacebookClient(facebookEventMessage.AccessToken);

            var postParams = new
            {
                name = facebookEventMessage.Invite.Title,
                caption = "You're all invited to participate",
                description = facebookEventMessage.Invite.Description,
                location = facebookEventMessage.Invite.Where,
                start_time = facebookEventMessage.Invite.Start,
                
            };

            facebookClient.Post("/me/events", postParams);
        }

        public void CreateFacebookPost(FacebookEventMessage facebookEventMessage)
        {
            var facebookClient = new FacebookClient(facebookEventMessage.AccessToken);

            var description = string.IsNullOrEmpty(facebookEventMessage.Invite.Description)
                ? "Event sent by iMeet.io"
                : facebookEventMessage.Invite.Description;

            var postParams = new
            {
                name = facebookEventMessage.Invite.Title,
                caption = "You're all invited to participate",
                description = description,
                link = "http://imeet.io/",
                picture = "http://imeet.io/favicon.ico"

            };
            
            facebookClient.Post("/me/feed", postParams);
        }
    }
}
