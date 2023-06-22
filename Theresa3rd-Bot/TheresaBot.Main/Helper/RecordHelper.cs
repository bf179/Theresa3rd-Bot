﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheresaBot.Main.Business;
using TheresaBot.Main.Command;

namespace TheresaBot.Main.Helper
{
    public static class RecordHelper
    {
        private static readonly RecordBusiness recordBusiness = new RecordBusiness();

        public static async Task AddImageRecords(List<string> imageUrls, long msgId, long groupId, long memberId)
        {
            try
            {
                if (imageUrls == null || imageUrls.Count == 0) return;
                await recordBusiness.AddImageRecord(imageUrls, msgId, groupId, memberId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public static async Task AddPlainRecords(List<string> plainMessages, long msgId, long groupId, long memberId)
        {
            try
            {
                if (plainMessages == null || plainMessages.Count == 0) return;
                string plainMessage = string.Join(' ', plainMessages);
                if (string.IsNullOrWhiteSpace(plainMessage)) return;
                await recordBusiness.AddMessageRecord(plainMessage, msgId, groupId, memberId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

    }
}
