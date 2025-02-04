﻿namespace TheresaBot.Main.Model.Pixiv
{
    public class PixivTags
    {
        public List<PixivTagModel> tags { get; set; }

        public List<string> getTags()
        {
            if (tags is null) return new List<string>();
            List<string> tagList = tags.Select(o => o.tag).Where(o => o is not null).ToList();
            return tagList.Where(o => string.IsNullOrWhiteSpace(o) == false).ToList();
        }

        public List<string> getFullTags()
        {
            if (tags is null) return new List<string>();
            List<string> tagList = new List<string>();
            tagList.AddRange(tags.Select(o => o.tag).Where(o => o is not null).ToList());
            tagList.AddRange(tags.Select(o => o.translation?.en).Where(o => o != null).ToList());
            tagList.AddRange(tags.Select(o => o.translation?.ko).Where(o => o != null).ToList());
            tagList.AddRange(tags.Select(o => o.translation?.zh).Where(o => o != null).ToList());
            tagList.AddRange(tags.Select(o => o.translation?.zh_tw).Where(o => o != null).ToList());
            return tagList.Where(o => string.IsNullOrWhiteSpace(o) == false).ToList();
        }
    }

    public class PixivTagModel
    {
        public string tag { get; set; }
        public PixivTagTranslation translation { get; set; }
    }

    public class PixivTagTranslation
    {
        public string en { get; set; }
        public string ko { get; set; }
        public string zh { get; set; }
        public string zh_tw { get; set; }
    }


}
