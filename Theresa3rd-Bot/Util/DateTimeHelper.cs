﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theresa3rd_Bot.Util
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取今天开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime getTodayStart()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// 获取今天结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime getTodayEnd()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        public static string getTimeStrBySeconds(int totalSecond)
        {
            string hour = (totalSecond / 3600).ToString().PadLeft(2, '0');
            string minute = ((totalSecond % 3600) / 60).ToString().PadLeft(2, '0');
            string second = (totalSecond % 60).ToString().PadLeft(2, '0');
            return string.Format("{0}:{1}:{2}", hour, minute, second);
        }

        /// <summary>
        /// 获取两个日期的秒数差值
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public static int GetSecondDiff(DateTime timeStart, DateTime timeEnd)
        {
            TimeSpan timeSpanStart = new TimeSpan(timeStart.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(timeEnd.Ticks);
            TimeSpan timeDiff = timeSpanEnd.Subtract(timeSpanStart);
            return (int)timeDiff.TotalSeconds;
        }

        public static TimeSpan GetTimeSpanDiff(DateTime timeStart, DateTime timeEnd)
        {
            TimeSpan timeSpanStart = new TimeSpan(timeStart.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(timeEnd.Ticks);
            return timeSpanEnd.Subtract(timeSpanStart);
        }

        /// <summary>
        /// 获取两个日期的天数差值
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public static int GetDayDiff(DateTime timeStart, DateTime timeEnd)
        {
            TimeSpan timeSpanStart = new TimeSpan(timeStart.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(timeEnd.Ticks);
            TimeSpan timeDiff = timeSpanEnd.Subtract(timeSpanStart);
            return (int)timeDiff.TotalDays;
        }

        /// <summary>
        /// 获取当前时间n秒后的实例
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static DateTime getDateTimeAfterMinutes(int minutes)
        {
            return DateTime.Now.AddMinutes(minutes);
        }

        /// <summary>
        /// 时间戳计时开始时间
        /// </summary>
        private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// DateTime转换为10位时间戳（单位：秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>10位时间戳（单位：秒）</returns>
        public static long dateTimeToTimeStamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
        }

        /// <summary>
        /// DateTime转换为13位时间戳（单位：毫秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>13位时间戳（单位：毫秒）</returns>
        public static long dateTimeToLongTimeStamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 10位时间戳（单位：秒）转换为DateTime
        /// </summary>
        /// <param name="timeStamp">10位时间戳（单位：秒）</param>
        /// <returns>DateTime</returns>
        public static DateTime timeStampToDateTime(long timeStamp)
        {
            return timeStampStartTime.AddSeconds(timeStamp).ToLocalTime();
        }

        /// <summary>
        /// 13位时间戳（单位：毫秒）转换为DateTime
        /// </summary>
        /// <param name="longTimeStamp">13位时间戳（单位：毫秒）</param>
        /// <returns>DateTime</returns>
        public static DateTime longTimeStampToDateTime(long longTimeStamp)
        {
            return timeStampStartTime.AddMilliseconds(longTimeStamp).ToLocalTime();
        }

        /// <summary>
        /// 获取当天是本星期的第几天
        /// </summary>
        /// <returns></returns>
        public static int getDayOfWeek()
        {
            DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Monday) return 1;
            if (dayOfWeek == DayOfWeek.Tuesday) return 2;
            if (dayOfWeek == DayOfWeek.Wednesday) return 3;
            if (dayOfWeek == DayOfWeek.Thursday) return 4;
            if (dayOfWeek == DayOfWeek.Friday) return 5;
            if (dayOfWeek == DayOfWeek.Saturday) return 6;
            if (dayOfWeek == DayOfWeek.Sunday) return 7;
            return 0;
        }

    }
}