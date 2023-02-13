﻿namespace TheresaBot.Main.Model.Config
{
    public class BaseSubscribeConfig : BasePluginConfig
    {
        public List<string> AddCommands { get; set; }

        public List<string> RmCommands { get; set; }

        public string Template { get; set; }

        public int ScanInterval { get; set; }
    }
}