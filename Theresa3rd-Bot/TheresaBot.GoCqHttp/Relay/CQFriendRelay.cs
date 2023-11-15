﻿using EleCho.GoCqHttpSdk.Message;
using EleCho.GoCqHttpSdk.Post;
using TheresaBot.Main.Relay;

namespace TheresaBot.GoCqHttp.Relay
{
    public class CQFriendRelay : FriendRelay
    {
        public CqPrivateMessagePostContext Args { get; set; }

        public CQFriendRelay(CqPrivateMessagePostContext args, int msgId, string message, long memberId) : base(msgId, message, memberId)
        {
            Args = args;
        }

        public override List<string> GetImageUrls()
        {
            return Args.Message.OfType<CqImageMsg>().Select(o => o.Url?.ToString()).Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
        }

    }
}
