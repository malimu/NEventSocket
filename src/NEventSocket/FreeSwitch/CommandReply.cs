﻿namespace NEventSocket.FreeSwitch
{
    using System;

    using NEventSocket.Util;

    /// <summary>
    /// CommandResponses contain the status in the Reply-Text header.
    /// </summary>
    [Serializable]
    public class CommandReply : BasicMessage
    {
        public CommandReply(BasicMessage basicMessage)
        {
            if (basicMessage.ContentType != ContentTypes.CommandReply)
                throw new ArgumentException(
                    "Expected content type command/reply, got {0} instead.".Fmt(basicMessage.ContentType));


            Headers = basicMessage.Headers;
            BodyText = basicMessage.BodyText;
        }

        public bool Success
        {
            get { return this.ReplyText != null && this.ReplyText[0] == '+'; }
        }

        public string ReplyText
        {
            get { return this.Headers["Reply-Text"]; }
        }
    }
}