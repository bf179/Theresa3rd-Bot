﻿namespace TheresaBot.Main.Relay
{
    public abstract class BaseRelay
    {
        public long MsgId { get; init; }

        public string Message { get; init; }

        public long MemberId { get; init; }

        public abstract List<string> GetImageUrls();

        public BaseRelay(long msgId, string message, long memberId)
        {
            this.MsgId = msgId;
            this.Message = message?.Trim() ?? string.Empty;
            this.MemberId = memberId;
        }

    }
}
