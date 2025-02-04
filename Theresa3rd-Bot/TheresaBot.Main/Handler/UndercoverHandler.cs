﻿using TheresaBot.Main.Cache;
using TheresaBot.Main.Command;
using TheresaBot.Main.Common;
using TheresaBot.Main.Exceptions;
using TheresaBot.Main.Game.Undercover;
using TheresaBot.Main.Helper;
using TheresaBot.Main.Model.Content;
using TheresaBot.Main.Relay;
using TheresaBot.Main.Reporter;
using TheresaBot.Main.Services;
using TheresaBot.Main.Session;
using TheresaBot.Main.Type.GameOptions;

namespace TheresaBot.Main.Handler
{
    internal class UndercoverHandler : GameHandler
    {
        private UCWordService ucWordService;

        public UndercoverHandler(BaseSession session, BaseReporter reporter) : base(session, reporter)
        {
            ucWordService = new UCWordService();
        }

        public async Task CreateUndercover(GroupCommand command)
        {
            try
            {
                UCGame ucGame;
                var gameMode = await AskGameModeAsync(command);
                if (gameMode == UCGameMode.Free)
                {
                    int[] scales = await AskCampScales(command);
                    ucGame = new UCGame(command, Session, Reporter, scales);
                }
                else if (gameMode == UCGameMode.Customize)
                {
                    int[] nums = await AskCharacterNums(command);
                    ucGame = new UCGame(command, Session, Reporter, nums[0], nums[1], nums[2]);
                }
                else
                {
                    ucGame = new UCGame(command, Session, Reporter);
                }
                await Task.Delay(1000);
                GameCahce.CreateGame(command, ucGame);
                await ucGame.StartProcessing();
            }
            catch (ProcessException ex)
            {
                await command.ReplyGroupMessageWithAtAsync(ex.RemindMessage);
            }
            catch (GameException ex)
            {
                await command.ReplyGroupMessageWithQuoteAsync(ex.RemindMessage);
            }
            catch (Exception ex)
            {
                await LogAndReplyError(command, ex, "Undercover游戏创建异常");
            }
        }

        public async Task SendPrivateWords(GroupCommand command)
        {
            try
            {
                var game = GameCahce.GetGameByGroup(command.GroupId);
                if (game is null || game.IsEnded || game is not UCGame ucGame)
                {
                    await command.ReplyGroupMessageWithQuoteAsync("游戏未开始，无法获取词条");
                    return;
                }
                var player = ucGame.GetPlayer(command.MemberId);
                if (player is null)
                {
                    await command.ReplyGroupMessageWithQuoteAsync("你还未加入游戏，无法获取词条");
                    return;
                }
                if (player.PlayerCamp == UCCamp.None)
                {
                    await command.ReplyGroupMessageWithQuoteAsync("词条还未派发，请耐心等待...");
                    return;
                }
                var ucConfig = BotConfig.GameConfig.Undercover;
                var contents = new List<BaseContent> {
                    new PlainContent(player.GetWordMessage(ucConfig.SendIdentity))
                };
                await command.SendTempMessageAsync(contents);
                await Task.Delay(1000);
                await command.ReplyGroupMessageWithQuoteAsync("词条已私发，请查看私聊消息");
            }
            catch (Exception ex)
            {
                await LogAndReplyError(command, ex, "获取词条异常");
            }
        }

        public async Task CreateWords(PrivateCommand command)
        {
            try
            {
                var memberId = command.MemberId;
                var isManager = memberId.IsSuperManager();
                var limitCount = BotConfig.GameConfig.Undercover.AddWordLimits;
                if (isManager == false && limitCount <= 0)
                {
                    await command.ReplyFriendMessageAsync($"超级管理员已关闭添加词条功能，请联系超级管理员~");
                    return;
                }
                var unauthCount = ucWordService.GetUnauthorizedCount(command.MemberId);
                if (isManager == false && unauthCount >= limitCount)
                {
                    await command.ReplyFriendMessageAsync($"已添加{unauthCount}个未经审核的词条，请等待超级管理员审核后再继续添加~");
                    return;
                }
                var newWords = await AskNewWords(command);
                if (isManager == false && unauthCount + newWords.Count > limitCount)
                {
                    await command.ReplyFriendMessageAsync($"非超级管理员限制添加词条个数为{limitCount}个，剩余可添加词条{limitCount - unauthCount}个，等待管理员审核后可以添加更多词条");
                    return;
                }
                foreach (var item in newWords)
                {
                    if (ucWordService.CheckWordExist(item)) continue;
                    ucWordService.InsertWords(item[0], item[1], memberId);
                }
                if (isManager)
                {
                    await command.ReplyFriendMessageAsync("添加完毕~");
                }
                else
                {
                    await command.ReplyFriendMessageAsync("添加完毕，请等待超级管理员审核~");
                }
            }
            catch (ProcessException ex)
            {
                await command.ReplyFriendMessageAsync(ex.RemindMessage);
            }
            catch (Exception ex)
            {
                await LogAndReplyError(command, ex, "词条添加异常");
            }
        }

        private async Task<UCGameMode> AskGameModeAsync(GroupCommand command)
        {
            var processInfo = ProcessCache.CreateProcess(command);
            var modeStep = processInfo.CreateStep($"请在60秒内发送数字选择游戏模式\r\n{EnumHelper.UCGameModes.JoinToString()}", CheckUCModeAsync);
            await processInfo.StartProcessing();
            return modeStep.Answer;
        }

        private async Task<int[]> AskCharacterNums(GroupCommand command)
        {
            var processInfo = ProcessCache.CreateProcess(command);
            var modeStep = processInfo.CreateStep($"请在60秒内发送【平民 卧底 白板】的数量，每个数字之间用空格隔开，比如：4 1 0", CheckCharacterNumsAsync);
            await processInfo.StartProcessing();
            return modeStep.Answer;
        }

        private async Task<int[]> AskCampScales(GroupCommand command)
        {
            var processInfo = ProcessCache.CreateProcess(command);
            var modeStep = processInfo.CreateStep($"请在60秒内发送【平民 卧底 白板】的比例，每个数字之间用空格隔开，比如：4 1 0", CheckCampScalesAsync);
            await processInfo.StartProcessing();
            return modeStep.Answer;
        }

        private async Task<List<string[]>> AskNewWords(PrivateCommand command)
        {
            var processInfo = ProcessCache.CreateProcess(command);
            var modeStep = processInfo.CreateStep($"请在60秒内发送词条，每个词条之间用空格隔开，多个词组之间换行隔开，比如：\r\n牛奶 豆浆\r\n苹果 雪梨", CheckNewWordsAsync);
            await processInfo.StartProcessing();
            return modeStep.Answer;
        }

        private async Task<UCGameMode> CheckUCModeAsync(string value)
        {
            int modeId;
            if (int.TryParse(value, out modeId) == false)
            {
                throw new ProcessException("模式不在范围内");
            }
            if (Enum.IsDefined(typeof(UCGameMode), modeId) == false)
            {
                throw new ProcessException("模式不在范围内");
            }
            return await Task.FromResult((UCGameMode)modeId);
        }

        private async Task<int[]> CheckCharacterNumsAsync(string value)
        {
            int civNum = 0, ucNum = 0, wbNum = 0;
            var splitArr = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (splitArr.Length < 1)
            {
                throw new ProcessException("必须指定平民的数量");
            }
            if (splitArr.Length > 0 && !int.TryParse(splitArr[0], out civNum))
            {
                throw new ProcessException("平民数量必须为数字");
            }
            if (splitArr.Length > 1 && !int.TryParse(splitArr[1], out ucNum))
            {
                throw new ProcessException("卧底数量必须为数字");
            }
            if (splitArr.Length > 2 && !int.TryParse(splitArr[2], out wbNum))
            {
                throw new ProcessException("白板数量必须为数字");
            }
            if (civNum < 2)
            {
                throw new ProcessException("平民数量至少需要在2个及以上");
            }
            if (ucNum < 1)
            {
                throw new ProcessException("卧底数量至少需要在1个及以上");
            }
            if (civNum + ucNum + wbNum < 3)
            {
                throw new ProcessException("平民+卧底+白板数量必须在3人及以上");
            }
            return await Task.FromResult(new int[] { civNum, ucNum, wbNum });
        }

        private async Task<int[]> CheckCampScalesAsync(string value)
        {
            int civNum = 0, ucNum = 0, wbNum = 0;
            var splitArr = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (splitArr.Length < 3)
            {
                throw new ProcessException("必须指定每个阵营的人数比例");
            }
            if (splitArr.Length > 0 && !int.TryParse(splitArr[0], out civNum))
            {
                throw new ProcessException("平民比例必须为数字");
            }
            if (splitArr.Length > 1 && !int.TryParse(splitArr[1], out ucNum))
            {
                throw new ProcessException("卧底比例必须为数字");
            }
            if (splitArr.Length > 2 && !int.TryParse(splitArr[2], out wbNum))
            {
                throw new ProcessException("白板比例必须为数字");
            }
            if (civNum < 2)
            {
                throw new ProcessException("平民比例至少在2及以上");
            }
            if (ucNum < 1)
            {
                throw new ProcessException("卧底比例至少在1及以上");
            }
            if (civNum + ucNum + wbNum < 3)
            {
                throw new ProcessException("平民+卧底+白板比例必须在3及以上");
            }
            return await Task.FromResult(new int[] { civNum, ucNum, wbNum });
        }

        private async Task<List<string[]>> CheckNewWordsAsync(BaseRelay relay)
        {
            var newWords = new List<string[]>();
            var splitArr = relay.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in splitArr)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                var words = item.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 2) throw new ProcessException($"词组 {item} 格式不正确，添加失败");
                if (words[0].Length == 0) throw new ProcessException($"词组 {item} 格式不正确，添加失败");
                if (words[1].Length == 0) throw new ProcessException($"词组 {item} 格式不正确，添加失败");
                newWords.Add(words);
            }
            if (newWords.Count == 0)
            {
                throw new ProcessException("没有检测到词条，请重新发送");
            }
            return await Task.FromResult(newWords);
        }


    }
}
