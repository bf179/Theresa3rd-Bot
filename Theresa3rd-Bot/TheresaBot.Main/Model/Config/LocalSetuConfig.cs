﻿namespace TheresaBot.Main.Model.Config
{
    public record LocalSetuConfig : BasePluginConfig
    {
        public List<string> Commands { get; set; }

        public string LocalPath { get; set; }

        public string Template { get; set; }

        public override BasePluginConfig FormatConfig()
        {
            Commands = Commands ?? new();
            return this;
        }

    }
}
