﻿using Mirai.CSharp.HttpApi.Models.ChatMessages;
using Mirai.CSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Theresa3rd_Bot.Common;
using Theresa3rd_Bot.Dao;
using Theresa3rd_Bot.Model.Mys;
using Theresa3rd_Bot.Model.PO;
using Theresa3rd_Bot.Model.Subscribe;
using Theresa3rd_Bot.Type;
using Theresa3rd_Bot.Util;

namespace Theresa3rd_Bot.Business
{
    public class MYSBusiness
    {
        private SubscribeDao subscribeDao;
        private SubscribeGroupDao subscribeGroupDao;
        private SubscribeRecordDao subscribeRecordDao;

        public MYSBusiness()
        {
            subscribeDao = new SubscribeDao();
            subscribeGroupDao = new SubscribeGroupDao();
            subscribeRecordDao = new SubscribeRecordDao();
        }

        /// <summary>
        /// 获取某个群已订阅的列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="subscribeCode"></param>
        /// <returns></returns>
        public List<SubscribePO> getSubscribeList(long groupId, string subscribeCode)
        {
            List<SubscribePO> subscribeList = new List<SubscribePO>();
            List<SubscribePO> dbSubscribes = subscribeDao.getSubscribes(subscribeCode, SubscribeType.米游社用户);
            if (dbSubscribes == null || dbSubscribes.Count == 0) return subscribeList;
            foreach (var item in dbSubscribes)
            {
                if (subscribeGroupDao.getCountBySubscribe(groupId, item.Id) == 0) continue;
                subscribeList.Add(item);
            }
            return subscribeList;
        }

        /// <summary>
        /// 删除一个订阅编码下的所有订阅
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="subscribeCode"></param>
        public void delAllSubscribe(long groupId, string subscribeCode)
        {
            List<SubscribePO> dbSubscribes = subscribeDao.getSubscribes(subscribeCode, SubscribeType.米游社用户);
            foreach (var item in dbSubscribes) subscribeGroupDao.delSubscribeGroup(groupId, item.Id);
        }


        public async Task<List<MysSubscribe>> getMysUserSubscribeAsync(SubscribeInfo subscribeInfo, int getCount = 2)
        {
            List<MysSubscribe> mysSubscribeList = new List<MysSubscribe>();
            List<MysResult<MysPostDataDto>> postDataList = new List<MysResult<MysPostDataDto>>();
            foreach (var item in Enum.GetValues(typeof(MysSectionType)))
            {
                int typeId = (int)item;
                if (typeId == (int)MysSectionType.全部) continue;
                if (subscribeInfo.SubscribeSubType != (int)MysSectionType.全部 && subscribeInfo.SubscribeSubType != typeId) continue;
                postDataList.Add(await getMysUserPostDtoAsync(subscribeInfo.SubscribeCode, typeId));
                await Task.Delay(1000);
            }

            foreach (var mysPostInfo in postDataList)
            {
                int index = 0;
                if (mysPostInfo.data.list == null || mysPostInfo.data.list.Count == 0) continue;
                foreach (var item in mysPostInfo.data.list)
                {
                    if (++index > getCount) break;
                    int shelfLife = BotConfig.SubscribeConfig.Mihoyo.ShelfLife;
                    DateTime createTime = DateTimeHelper.UnixTimeStampToDateTime(item.post.created_at);
                    if (shelfLife > 0 && createTime < DateTime.Now.AddSeconds(-1 * shelfLife)) continue;

                    SubscribeRecordPO subscribeRecord = new SubscribeRecordPO(subscribeInfo.SubscribeId);
                    subscribeRecord.Title = item.post.subject?.filterEmoji().cutString(200);
                    subscribeRecord.Content = item.post.content?.filterEmoji().cutString(500);
                    subscribeRecord.CoverUrl = item.post.images.Count > 0 ? item.post.images[0] : "";
                    subscribeRecord.LinkUrl = HttpUrl.getMysArticleUrl(item.post.post_id);
                    subscribeRecord.DynamicCode = item.post.post_id;
                    subscribeRecord.DynamicType = SubscribeDynamicType.帖子;

                    SubscribeRecordPO dbSubscribe = subscribeRecordDao.checkExists(subscribeInfo.SubscribeId, item.post.post_id);
                    if (dbSubscribe != null) continue;

                    MysSubscribe mysSubscribe = new MysSubscribe();
                    mysSubscribe.SubscribeRecord = subscribeRecordDao.Insert(subscribeRecord);
                    mysSubscribe.MysUserPostDto = item;
                    mysSubscribe.CreateTime = createTime;
                    mysSubscribeList.Add(mysSubscribe);
                    await Task.Delay(1000);
                }
            }
            return mysSubscribeList;
        }


        public async Task<List<IChatMessage>> getSubscribeInfoAsync(MysSubscribe mysSubscribe, string template = "")
        {
            if (string.IsNullOrWhiteSpace(template)) return await getDefaultSubscribeInfoAsync(mysSubscribe);
            template = template.Replace("{UserName}", mysSubscribe.MysUserPostDto.user.nickname);
            template = template.Replace("{CreateTime}", mysSubscribe.CreateTime.ToSimpleString());
            template = template.Replace("{Title}", mysSubscribe.SubscribeRecord.Title);
            template = template.Replace("{Content}", mysSubscribe.SubscribeRecord.Content.cutString(200));
            template = template.Replace("{Urls}", mysSubscribe.SubscribeRecord.LinkUrl);
            List<IChatMessage> chailList = new List<IChatMessage>();
            chailList.Add(new PlainMessage(template));
            FileInfo fileInfo = string.IsNullOrEmpty(mysSubscribe.SubscribeRecord.CoverUrl) ? null : await HttpHelper.DownImgAsync(mysSubscribe.SubscribeRecord.CoverUrl);
            if (fileInfo != null) chailList.Add((IChatMessage)await MiraiHelper.Session.UploadPictureAsync(UploadTarget.Group, fileInfo.FullName));
            return chailList;
        }

        public async Task<List<IChatMessage>> getDefaultSubscribeInfoAsync(MysSubscribe mysSubscribe)
        {
            List<IChatMessage> chailList = new List<IChatMessage>();
            chailList.AddRange(await getDefaultPostInfoAsync(mysSubscribe));
            return chailList;
        }


        public async Task<List<IChatMessage>> getDefaultPostInfoAsync(MysSubscribe mysSubscribe)
        {
            List<IChatMessage> chailList = new List<IChatMessage>();
            chailList.Add(new PlainMessage($"{mysSubscribe.SubscribeRecord.Title}\r\n"));
            chailList.Add(new PlainMessage($"{mysSubscribe.SubscribeRecord.Content.cutString(200)}\r\n"));
            FileInfo fileInfo = string.IsNullOrEmpty(mysSubscribe.SubscribeRecord.CoverUrl) ? null : await HttpHelper.DownImgAsync(mysSubscribe.SubscribeRecord.CoverUrl);
            if (fileInfo != null) chailList.Add((IChatMessage)await MiraiHelper.Session.UploadPictureAsync(UploadTarget.Group, fileInfo.FullName));
            chailList.Add(new PlainMessage($"{mysSubscribe.SubscribeRecord.LinkUrl}"));
            return chailList;
        }


        /*-------------------------------------------------------------接口相关--------------------------------------------------------------------------*/

        public async Task<MysResult<MysPostDataDto>> getMysUserPostDtoAsync(string userId, int gids)
        {
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            string getUrl = HttpUrl.getMysPostListUrl(userId, gids);
            string json = await HttpHelper.HttpGetAsync(getUrl, headerDic);
            return JsonConvert.DeserializeObject<MysResult<MysPostDataDto>>(json);
        }


        public async Task<MysResult<MysUserFullInfoDto>> geMysUserFullInfoDtoAsync(string userId, int gids)
        {
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            string getUrl = HttpUrl.getMystUserFullInfo(userId, gids);
            string json = await HttpHelper.HttpGetAsync(getUrl, headerDic);
            return JsonConvert.DeserializeObject<MysResult<MysUserFullInfoDto>>(json);
        }


    }
}
