﻿using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls;

namespace Denrage.AchievementTrackerModule.Services.Factories.AchievementControl
{
    public class AchievementCollectionControlFactory : AchievementControlFactory<AchievementCollectionControl, CollectionDescription>
    {
        private readonly IAchievementService achievementService;
        private readonly IItemDetailWindowManager itemDetailWindowManager;
        private readonly ContentsManager contentsManager;

        public AchievementCollectionControlFactory(IAchievementService achievementService, IItemDetailWindowManager itemDetailWindowManager, ContentsManager contentsManager)
        {
            this.achievementService = achievementService;
            this.itemDetailWindowManager = itemDetailWindowManager;
            this.contentsManager = contentsManager;
        }

        protected override AchievementCollectionControl CreateInternal(AchievementTableEntry achievement, CollectionDescription description)
            => new AchievementCollectionControl(this.itemDetailWindowManager, this.achievementService, this.contentsManager, achievement, description);
    }
}