﻿using System;
using System.Collections.Generic;
using static Denrage.AchievementTrackerModule.Libs.Achievement.CollectionAchievementTable;

namespace Denrage.AchievementTrackerModule.Interfaces
{
    public interface IItemDetailWindowManager : IDisposable
    {
        void CreateAndShowWindow(string name, string[] columns, List<CollectionAchievementTableEntry> item, string achievementLink, int achievementId, int itemIndex);

        bool ShowWindow(string name);

        void Update();
    }
}