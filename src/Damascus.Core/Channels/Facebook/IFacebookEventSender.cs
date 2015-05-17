 using System;
using Damascus.Message;
using Damascus.Message.Command;

namespace Damascus.Core
{
    public interface IFacebookEventSender
    {
        void CreateFacebookEvent(FacebookEventMessage facebookEventMessage);
        void CreateFacebookPost(FacebookEventMessage facebookEventMessage);
    }
}