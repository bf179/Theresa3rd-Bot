﻿using System.Timers;
using TheresaBot.Main.Common;
using TheresaBot.Main.Datas;
using TheresaBot.Main.Handler;
using TheresaBot.Main.Helper;
using TheresaBot.Main.Model.Pixiv;
using TheresaBot.Main.Reporter;
using TheresaBot.Main.Session;

namespace TheresaBot.Main.Timers
{
    internal static class PixivTagScanTimer
    {
        private static BaseSession Session;
        private static BaseReporter Reporter;
        private static System.Timers.Timer SystemTimer;

        public static void Init(BaseSession session, BaseReporter reporter)
        {
            Destroy();
            Session = session;
            Reporter = reporter;
            SystemTimer = new System.Timers.Timer();
            SystemTimer.Interval = BotConfig.SubscribeConfig.PixivTag.ScanInterval * 1000;
            SystemTimer.AutoReset = true;
            SystemTimer.Elapsed += new ElapsedEventHandler(HandlerMethod);
            SystemTimer.Enabled = true;
        }

        public static bool Destroy()
        {
            if (SystemTimer is null) return false;
            if (SystemTimer.Enabled == false) return false;
            SystemTimer.Enabled = false;
            SystemTimer.Stop();
            SystemTimer.Close();
            SystemTimer.Dispose();
            SystemTimer = null;
            return true;
        }

        private static void HandlerMethod(object source, ElapsedEventArgs e)
        {
            try
            {
                SystemTimer.Enabled = false;
                if (BusinessHelper.IsPixivCookieAvailable() == false)
                {
                    LogHelper.Info("Pixiv Cookie过期或不可用，已停止扫描pixiv标签最新作品，请更新Cookie...");
                    return;
                }
                LogHelper.Info($"开始扫描pixiv标签最新作品...");
                PixivTagScanReport report = new PixivPushHandler(Session, Reporter).HandleTagPushAsync().Result;
                CountDatas.AddPixivScanTimes(report);
                CountDatas.AddPixivPushTimes(report);
                LogHelper.Info($"pixiv标签扫描完毕，扫描标签/扫描作品/失败标签/失败作品={report.ScanTag}/{report.ScanWork}/{report.ErrorTag}/{report.ErrorWork}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "PixivTagTimer异常");
                Reporter.SendError(ex, "PixivTagTimer异常").Wait();
            }
            finally
            {
                SystemTimer.Enabled = true;
            }
        }


    }
}
