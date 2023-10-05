﻿using TheresaBot.Main.Helper;

namespace TheresaBot.Main.Model.Config
{
    public record GeneralConfig : BaseConfig
    {
        public List<string> Prefixs { get; set; }

        public string TempPath { get; set; }

        public string FontPath { get; set; }

        public List<long> ErrorGroups { get; set; }

        public string ErrorMsg { get; set; }

        public string ClearCron { get; set; }

        public string ErrorImgPath { get; set; }

        public string DisableMsg { get; set; }

        public string NoPermissionsMsg { get; set; }

        public string ManagersRequiredMsg { get; set; }

        public string SetuCustomDisableMsg { get; set; }

        public bool SendRelevantCommands { get; set; }

        public string DefaultPrefix => Prefixs.FirstOrDefault() ?? string.Empty;

        public override GeneralConfig FormatConfig()
        {
            Prefixs = Prefixs?.Trim() ?? new();
            ErrorGroups = ErrorGroups ?? new();
            ClearCron = string.IsNullOrWhiteSpace(ClearCron) ? "0 0 4 * * ?" : ClearCron;
            return this;
        }

    }
}
