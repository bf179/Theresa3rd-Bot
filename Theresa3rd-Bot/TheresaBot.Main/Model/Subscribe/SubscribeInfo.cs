﻿using TheresaBot.Main.Type;

namespace TheresaBot.Main.Model.Subscribe
{
    public class SubscribeInfo
    {
        public int SubscribeId { get; set; }
        public string SubscribeCode { get; set; }
        public SubscribeType SubscribeType { get; set; }
        public int SubscribeSubType { get; set; }
        public string SubscribeName { get; set; }
        public string SubscribeDescription { get; set; }
        public long GroupId { get; set; }
    }

}