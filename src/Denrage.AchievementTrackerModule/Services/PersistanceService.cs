﻿using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Denrage.AchievementTrackerModule.Services
{
    public class PersistanceService : IPersistanceService
    {
        private const string SAVE_FILE_NAME = "persistanceStorage.json";
        private Storage storage;
        private readonly DirectoriesManager directoriesManager;
        private readonly AchievementDetailsWindowManager achievementDetailsWindowManager;
        private readonly ItemDetailWindowManager itemDetailWindowManager;
        private readonly AchievementTrackerService achievementTrackerService;
        private readonly Logger logger;

        public PersistanceService(
            DirectoriesManager directoriesManager,
            AchievementDetailsWindowManager achievementDetailsWindowManager,
            ItemDetailWindowManager itemDetailWindowManager,
            AchievementTrackerService achievementTrackerService,
            Logger logger)
        {
            this.directoriesManager = directoriesManager;
            this.achievementDetailsWindowManager = achievementDetailsWindowManager;
            this.itemDetailWindowManager = itemDetailWindowManager;
            this.achievementTrackerService = achievementTrackerService;
            this.logger = logger;
        }

        public void Save(int achievementTrackWindowLocationX, int achievementTrackWindowLocationY, bool showTrackWindow)
        {
            var storage = new Storage();

            foreach (var item in this.achievementDetailsWindowManager.Windows.Where(x => x.Value.Visible))
            {
                storage.AchievementInformation[item.Key] = new AchievementWindowInformation()
                {
                    AchievementId = item.Value.AchievementId,
                    PositionX = item.Value.Location.X,
                    PositionY = item.Value.Location.Y,
                };
            }

            foreach (var item in this.itemDetailWindowManager.Windows.Where(x => x.Value.Window.Visible))
            {
                if (!storage.ItemInformation.TryGetValue(item.Value.AchievementId, out var itemWindows))
                {
                    itemWindows = new Dictionary<int, ItemInformation>();
                    storage.ItemInformation[item.Value.AchievementId] = itemWindows;
                }

                itemWindows[item.Value.ItemIndex] = new ItemInformation()
                {
                    AchievementId = item.Value.AchievementId,
                    Index = item.Value.ItemIndex,
                    Name = item.Value.Name,
                    PositionX = item.Value.Window.Location.X,
                    PositionY = item.Value.Window.Location.Y,
                };
            }

            storage.TrackedAchievements.AddRange(this.achievementTrackerService.ActiveAchievements);

            storage.TrackWindowLocationX = achievementTrackWindowLocationX;
            storage.TrackWindowLocationY = achievementTrackWindowLocationY;
            storage.ShowTrackWindow = showTrackWindow;

            try
            {
                var safeFolder = this.directoriesManager.GetFullDirectoryPath("achievement_module");

                _ = System.IO.Directory.CreateDirectory(safeFolder);

                System.IO.File.WriteAllText(System.IO.Path.Combine(safeFolder, SAVE_FILE_NAME), System.Text.Json.JsonSerializer.Serialize(storage));
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Exception occured on saving persistent information");
            }
        }

        public Storage Get()
        {
            try
            {
                var safeFolder = this.directoriesManager.GetFullDirectoryPath("achievement_module");
                var file = System.IO.Path.Combine(safeFolder, SAVE_FILE_NAME);
                if (this.storage is null)
                {
                    this.storage = System.IO.File.Exists(file)
                        ? System.Text.Json.JsonSerializer.Deserialize<Storage>(System.IO.File.ReadAllText(file))
                        : new Storage();
                }

                return this.storage;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Exception occured on reading persistent information");
                return new Storage();
            }
        }
    }
}