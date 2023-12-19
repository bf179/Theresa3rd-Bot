﻿using Mirai.CSharp.HttpApi.Models.ChatMessages;
using Mirai.CSharp.HttpApi.Models.EventArgs;
using TheresaBot.Main.Command;
using TheresaBot.Main.Model.Invoker;
using TheresaBot.Main.Session;
using TheresaBot.MiraiHttpApi.Helper;

namespace TheresaBot.MiraiHttpApi.Command
{
    public class MiraiFriendCommand : PrivateCommand
    {
        private IFriendMessageEventArgs Args { get; init; }

        public override long MessageId => Args.GetMessageId();

        public override long MemberId => Args.Sender.Id;

        public override string MemberName => Args.Sender.Name;

        public MiraiFriendCommand(BaseSession baseSession, CommandHandler<PrivateCommand> invoker, IFriendMessageEventArgs args, string instruction, string command, string prefix)
            : base(baseSession, invoker, instruction, command, prefix)
        {
            this.Args = args;
        }

        public override List<string> GetImageUrls()
        {
            return Args.Chain.OfType<ImageMessage>().Select(o => o.Url).ToList();
        }

    }
}
