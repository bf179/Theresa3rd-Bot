﻿namespace TheresaBot.Main.Model.Config
{
    public class ManageConfig : BaseConfig
    {
        public List<string> PixivCookieCommands { get; private set; }

        public List<string> SaucenaoCookieCommands { get; private set; }

        public List<string> DisableTagCommands { get; private set; }

        public List<string> EnableTagCommands { get; private set; }

        public List<string> DisableMemberCommands { get; private set; }

        public List<string> EnableMemberCommands { get; private set; }

        public List<string> ListSubCommands { get; private set; }

        public List<string> RemoveSubCommands { get; private set; }

        public List<string> TagSugarCommands { get; private set; }

        public override ManageConfig FormatConfig()
        {
            return this;
        }

    }
}
