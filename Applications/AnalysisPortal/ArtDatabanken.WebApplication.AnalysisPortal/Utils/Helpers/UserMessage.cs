using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    public class UserMessage
    {
        public string Message { get; set; }
        public UserMessageType MessageType { get; set; }

        public UserMessage(string message, UserMessageType messageType)
        {
            Message = message;
            MessageType = messageType;
        }

        public UserMessage(string message)
        {
            Message = message;
            MessageType = UserMessageType.Success;
        }
    }

    public enum UserMessageType
    {
        Success,
        Info,
        Warning,
        Error        
    }
}
