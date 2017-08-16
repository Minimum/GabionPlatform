﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.News;

namespace GabionPlatform.DTO.News
{
    public class NewsStatusDto : EditableGabionDto
    {
        public String Title { get; set; }
        public long TitleImage { get; set; }
        public NewsStatusType ContentType { get; set; }
        public String Content { get; set; }

        public NewsStatusDto()
        {
            Title = "";
            TitleImage = 0;
            ContentType = NewsStatusType.Standard;
            Content = "";
        }

        public NewsStatusDto(NewsStatus status)
            : base(status)
        {
            Title = status.Title;
            TitleImage = status.TitleImage;
            ContentType = status.ContentType;
            Content = status.Content;
        }

        public static List<NewsStatusDto> ConvertList(ICollection<NewsStatus> accounts)
        {
            List<NewsStatusDto> models = new List<NewsStatusDto>();

            foreach (NewsStatus account in accounts)
            {
                models.Add(new NewsStatusDto(account));
            }

            return models;
        }
    }
}