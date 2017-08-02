﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.DAL;
using GabionPlatform.Models;
using GabionPlatform.Settings;

namespace GabionPlatform.News
{
    public class NewsManager
    {
        public const String FlagChangeStatus = "NewsChangeStatus";
        public const String FlagChangeWeather = "NewsChangeWeather";

        public const String SettingCurrentStatus = "NewsCurrentStatus";
        public const String SettingWeatherLocation = "NewsWeatherLocation";
        public const String SettingWeatherUpdateInterval = "NewsWeatherUpdateInterval";

        protected PlatformContext Context;

        protected AppInstance Instance;

        public NewsManager(AppInstance instance)
        {
            Context = instance.Context;

            Instance = instance;
        }

        public void Install()
        {
            SettingsManager settings = Instance.Settings;

            settings.AddSetting(new PlatformSetting(SettingCurrentStatus, "Current News Status",
                "The current news status.", "0"));

            settings.AddSetting(new PlatformSetting(SettingWeatherLocation, "Weather ZIP Code",
                "The current ZIP code for the weather system.", "92656"));

            settings.AddSetting(new PlatformSetting(SettingWeatherUpdateInterval, "Weather Update Interval",
                "The update interval for the weather system in minutes.", "30"));

            return;
        }

        public NewsStatus GetStatusById(long id)
        {
            return Context.NewsStatus.SingleOrDefault(s => s.Id == id);
        }

        public long GetCurrentStatusId()
        {
            PlatformSetting setting = Instance.Settings.GetSettingByName(SettingCurrentStatus);
            long statusId = 0;

            if (setting != null)
            {
                statusId = setting.ToInt64();
            }

            return statusId;
        }

        public NewsStatus GetCurrentStatus()
        {
            return GetStatusById(GetCurrentStatusId());
        }

        public void AddStatus(NewsStatus status)
        {
            Context.NewsStatus.Add(status);

            return;
        }

        public List<NewsStatus> GetStatusList(int page, int pageSize)
        {
            return Context.NewsStatus.Where(s => s.Id > 0).OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public long GetStatusCount()
        {
            return Context.NewsStatus.LongCount();
        }

        public WeatherStatus GetCurrentWeatherStatus()
        {
            return Context.WeatherStatus.Where(s => s.Id > 0).OrderByDescending(s => s.Id).FirstOrDefault();
        }

        public void AddWeatherStatus(WeatherStatus status)
        {
            Context.WeatherStatus.Add(status);

            return;
        }
    }
}