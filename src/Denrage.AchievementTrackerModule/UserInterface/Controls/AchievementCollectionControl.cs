﻿using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{

    public class AchievementCollectionControl : AchievementListControl<CollectionDescription, CollectionDescriptionEntry>
    {
        public AchievementCollectionControl(
            IItemDetailWindowManager itemDetailWindowManager,
            IAchievementService achievementService,
            ContentsManager contentsManager,
            AchievementTableEntry achievement,
            CollectionDescription description)
            : base(itemDetailWindowManager, achievementService, contentsManager, achievement, description)
        {
        }

        protected override void ColorControl(Control control, bool achievementBitFinished)
        {
            if (control is Image image)
            {
                image.Tint = achievementBitFinished
                           ? Microsoft.Xna.Framework.Color.White
                           : Microsoft.Xna.Framework.Color.FromNonPremultiplied(255, 255, 255, 50);
            }
        }

        protected override Control CreateEntryControl(int index, CollectionDescriptionEntry entry, Container parent)
        {
            var spinner = new LoadingSpinner()
            {
                Parent = parent,
            };

            spinner.Location = new Point((parent.Width - spinner.Width) / 2, (parent.Height - spinner.Height) / 2);

            spinner.Show();

            return new Image()
            {
                Parent = parent,
                Width = 32,
                Height = 32,
                Texture = this.AchievementService.GetImage(entry.ImageUrl, () => spinner.Dispose()),
                ZIndex = 1,
            };
        }

        protected override string GetDisplayName(CollectionDescriptionEntry entry) => entry?.DisplayName ?? string.Empty;

        protected override IEnumerable<CollectionDescriptionEntry> GetEntries(CollectionDescription description) => description?.EntryList ?? System.Array.Empty<CollectionDescriptionEntry>().ToList();
    }
}