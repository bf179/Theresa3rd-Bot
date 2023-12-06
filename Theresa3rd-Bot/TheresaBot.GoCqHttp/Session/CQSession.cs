﻿using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using TheresaBot.GoCqHttp.Helper;
using TheresaBot.GoCqHttp.Result;
using TheresaBot.Main.Common;
using TheresaBot.Main.Model.Content;
using TheresaBot.Main.Model.Infos;
using TheresaBot.Main.Model.Result;
using TheresaBot.Main.Session;
using TheresaBot.Main.Type;

namespace TheresaBot.GoCqHttp.Session
{
    public class CQSession : BaseSession
    {
        public override PlatformType PlatformType => PlatformType.GoCQHttp;

        public override async Task<GroupInfos[]> LoadGroupInfosAsync()
        {
            return await CQHelper.LoadGroupInfosAsync();
        }

        public override async Task<BaseResult> SendGroupMessageAsync(long groupId, string message)
        {
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(message));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMessageAsync(long groupId, List<BaseContent> contents, List<long> atMembers = null, bool isAtAll = false)
        {
            if (contents.Count == 0) return BaseResult.Undo;
            List<CqMsg> msgList = new List<CqMsg>();
            if (isAtAll) msgList.Add(CqAtMsg.AtAll);
            if (atMembers is null) atMembers = new();
            foreach (long memberId in atMembers)
            {
                msgList.Add(new CqAtMsg(memberId));
                msgList.Add(new CqTextMsg(" "));
            }
            msgList.AddRange(contents.ToCQMessageAsync());
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMessageWithAtAsync(long groupId, long memberId, string message)
        {
            List<CqMsg> msgList = new List<CqMsg>();
            msgList.Add(new CqAtMsg(memberId));
            msgList.Add(new CqTextMsg(message));
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMessageWithAtAsync(long groupId, long memberId, List<BaseContent> contents)
        {
            List<CqMsg> msgList = new List<CqMsg>();
            msgList.Add(new CqAtMsg(memberId));
            msgList.AddRange(contents.ToCQMessageAsync());
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMessageWithQuoteAsync(long groupId, long memberId, long quoteMsgId, string message)
        {
            List<CqMsg> msgList = new List<CqMsg>();
            msgList.Add(new CqReplyMsg(quoteMsgId));
            msgList.Add(new CqTextMsg(" "));//避免Replay和At不能同时使用
            msgList.Add(new CqAtMsg(memberId));
            msgList.Add(new CqTextMsg(message));
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMessageWithQuoteAsync(long groupId, long memberId, long quoteMsgId, List<BaseContent> contents)
        {
            if (contents.Count == 0) return BaseResult.Undo;
            List<CqMsg> msgList = new List<CqMsg>();
            msgList.Add(new CqReplyMsg(quoteMsgId));
            msgList.Add(new CqTextMsg(" "));//避免Replay和At不能同时使用
            msgList.Add(new CqAtMsg(memberId));
            msgList.AddRange(contents.ToCQMessageAsync());
            var result = await CQHelper.Session.SendGroupMessageAsync(groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupMergeAsync(long groupId, List<BaseContent[]> contentLists)
        {
            if (contentLists.Count == 0) return BaseResult.Undo;
            var nodeList = contentLists.Select(o => new CqForwardMessageNode(BotConfig.BotName, BotConfig.BotQQ, new CqMessage(o.ToList().ToCQMessageAsync()))).ToList();
            var result = await CQHelper.Session.SendGroupForwardMessageAsync(groupId, new CqForwardMessage(nodeList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendGroupForwardAsync(long groupId, List<ForwardContent> contents)
        {
            if (contents.Count == 0) return BaseResult.Undo;
            var nodeList = new List<CqForwardMessageNode>();
            foreach (var content in contents)
            {
                if (content.Contents is null || content.Contents.Length == 0) continue;
                var memberId = content.MemberId <= 0 ? BotConfig.BotQQ : content.MemberId;
                var memberName = content.MemberName is null ? memberId.ToString() : content.MemberName;
                nodeList.Add(new CqForwardMessageNode(memberName, memberId, new CqMessage(content.Contents.ToList().ToCQMessageAsync())));
            }
            var result = await CQHelper.Session.SendGroupForwardMessageAsync(groupId, new CqForwardMessage(nodeList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendFriendMessageAsync(long memberId, string message)
        {
            var result = await CQHelper.Session.SendPrivateMessageAsync(memberId, new CqMessage(message));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendFriendMessageAsync(long memberId, List<BaseContent> contents)
        {
            if (contents.Count == 0) return BaseResult.Undo;
            CqMsg[] msgList = contents.ToCQMessageAsync();
            var result = await CQHelper.Session.SendPrivateMessageAsync(memberId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendTempMessageAsync(long groupId, long memberId, string message)
        {
            var result = await CQHelper.Session.SendPrivateMessageAsync(memberId, groupId, new CqMessage(message));
            return new CQResult(result, result.MessageId);
        }

        public override async Task<BaseResult> SendTempMessageAsync(long groupId, long memberId, List<BaseContent> contents)
        {
            if (contents.Count == 0) return BaseResult.Undo;
            CqMsg[] msgList = contents.ToCQMessageAsync();
            var result = await CQHelper.Session.SendPrivateMessageAsync(memberId, groupId, new CqMessage(msgList));
            return new CQResult(result, result.MessageId);
        }

        public override async Task RevokeGroupMessageAsync(long groupId, long messageId)
        {
            await CQHelper.Session.RecallMessageAsync(messageId);
        }

        public override async Task MuteGroupMemberAsync(long groupId, long memberId, int seconds)
        {
            var duration = TimeSpan.FromSeconds(seconds);
            await CQHelper.Session.BanGroupMemberAsync(groupId, memberId, duration);
        }

    }
}
