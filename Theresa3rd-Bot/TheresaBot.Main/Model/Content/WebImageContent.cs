﻿namespace TheresaBot.Main.Model.Content
{
    public class WebImageContent : ImageContent
    {
        public string Url { get; set; }

        public WebImageContent(string url)
        {
            this.Url = url;
        }

    }
}
