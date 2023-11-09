﻿using TheresaBot.Main.Common;
using TheresaBot.Main.Model.Config;
using TheresaBot.Main.Model.Infos;

namespace TheresaBot.Main.Helper
{
    public static class PermissionsHelper
    {
        /// <summary>
        /// 判断是否可以处理一个群的消息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsAuthorized(this long groupId)
        {
            PermissionsConfig permissionsConfig = BotConfig.PermissionsConfig;
            if (permissionsConfig is null) return false;
            List<long> acceptGroups = permissionsConfig.AcceptGroups;
            return acceptGroups.Contains(0) || acceptGroups.Contains(groupId);
        }

        /// <summary>
        /// 判断是否存在其中一个群需要显示AI图
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool IsShowAISetu(this List<long> groupIds)
        {
            return groupIds.Any(o => o.IsShowAISetu());
        }

        /// <summary>
        /// 判断某一个群是否需要显示AI图
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsShowAISetu(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuShowAIGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某一个群是否可以显示图片
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isR18Img"></param>
        /// <returns></returns>
        public static bool IsShowSetuImg(this long groupId, bool isR18Img)
        {
            var groups = BotConfig.PermissionsConfig.SetuShowImgGroups;
            if (groups.Contains(0) == false && groups.Contains(groupId) == false) return false;
            if (isR18Img && groupId.IsShowR18SetuImg() == false) return false;
            return true;
        }

        /// <summary>
        /// 判断是否存在其中一个群可以显示R18图片
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool IsShowR18SetuImg(this List<long> groupIds)
        {
            return groupIds.Any(o => o.IsShowR18SetuImg());
        }

        /// <summary>
        /// 判断某一个群是否可以显示R18图片
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsShowR18SetuImg(this long groupId)
        {
            var ShowR18Groups = BotConfig.PermissionsConfig.SetuShowR18Groups;
            var ShowR18ImgGroups = BotConfig.PermissionsConfig.SetuShowR18ImgGroups;
            if (ShowR18Groups.Contains(0) == false && ShowR18Groups.Contains(groupId) == false) return false;
            if (ShowR18ImgGroups.Contains(0) == false && ShowR18ImgGroups.Contains(groupId) == false) return false;
            return true;
        }

        /// <summary>
        /// 判断是否存在其中一个群可以显示R18内容
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool IsShowR18Setu(this List<long> groupIds)
        {
            return groupIds.Any(o => o.IsShowR18Setu());
        }

        /// <summary>
        /// 判断某一个群是否可以显示R18内容
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsShowR18Setu(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuShowR18Groups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某一个群是否可以显示R18的Saucenao的搜索结果
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsShowR18Saucenao(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SaucenaoR18Groups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某一个成员是否被设置为超级管理员
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static bool IsSuperManager(this long memberId)
        {
            return BotConfig.PermissionsConfig.SuperManagers.Contains(memberId);
        }

        /// <summary>
        /// 判断某一个群是否可以0CD获取涩图
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSetuNoneCD(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuNoneCDGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某一个成员是否可以无限次使用指令
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static bool IsLimitlessMember(this long memberId)
        {
            return BotConfig.PermissionsConfig.LimitlessMembers.Contains(memberId);
        }

        /// <summary>
        /// 判断某一个群是否可以0CD获取涩图
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSetuLimitless(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuNoneCDGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有涩图权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSetuAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有自定义涩图搜索权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSetuCustomAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SetuCustomGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有订阅权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSubscribeAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SubscribeGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有搜图权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsSaucenaoAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.SaucenaoGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有日榜权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsPixivRankingAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.PixivRankingGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 判断某个群是否拥有词云权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool IsWordCloudAuthorized(this long groupId)
        {
            var groups = BotConfig.PermissionsConfig.WordCloudGroups;
            return groups.Contains(0) || groups.Contains(groupId);
        }

        /// <summary>
        /// 根据配置文件中的一组群号，返回一组可发送消息的群号
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static List<long> ToSendableGroups(this List<long> groupIds)
        {
            var fullGroups = BotConfig.GroupInfos.Select(o => o.GroupId).ToList();
            var sendableGroups = groupIds.Where(o => groupIds.Contains(o)).ToList();
            return groupIds.Contains(0) ? fullGroups : sendableGroups;
        }

    }
}
