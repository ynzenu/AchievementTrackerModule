﻿using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Denrage.AchievementTrackerModule.Models.Achievement.CollectionAchievementTable;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
    public class ItemDetailWindow : WindowBase2
    {
        private readonly ContentsManager contentsManager;
        private readonly IAchievementService achievementService;
        private readonly IAchievementTableEntryProvider achievementTableEntryProvider;
        private readonly string name;
        private readonly string[] columns;
        private readonly List<CollectionAchievementTableEntry> item;
        private readonly Texture2D texture;

        public ItemDetailWindow(
            ContentsManager contentsManager,
            IAchievementService achievementService,
            IAchievementTableEntryProvider achievementTableEntryProvider,
            string name,
            string[] columns,
            List<CollectionAchievementTableEntry> item)
        {
            this.contentsManager = contentsManager;
            this.achievementService = achievementService;
            this.achievementTableEntryProvider = achievementTableEntryProvider;
            this.texture = this.contentsManager.GetTexture("156390.png");

            this.name = name;
            this.columns = columns;
            this.item = item;

            this.BuildWindow();
        }

        private void BuildWindow()
        {
            this.Title = this.name;
            this.ConstructWindow(this.texture, new Rectangle(0, 0, 600, 400), new Rectangle(0, 30, 600, 400 - 30));

            var panel = new FlowPanel()
            {
                Parent = this,
                Size = this.ContentRegion.Size,
                FlowDirection = ControlFlowDirection.TopToBottom,
            };

            for (var i = 0; i < this.item.Count; i++)
            {
                var innerPannel = new Panel()
                {
                    Parent = panel,
                    Width = panel.ContentRegion.Width,
                    HeightSizingMode = SizingMode.AutoSize,
                };

                var label = new Label()
                {
                    Parent = innerPannel,
                    Width = (int)System.Math.Floor(0.3 * innerPannel.ContentRegion.Width),
                    Text = this.columns[i],
                };

                var control = this.achievementTableEntryProvider.GetTableEntryControl(this.item[i]);

                if (control != null)
                {
                    control.Parent = innerPannel;
                    control.Width = innerPannel.Width - label.Width;
                    control.Location = new Point(label.Width, 0);
                }
            }
        }

        public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.DrawOnCtrl(this,
                                   this.texture,
                                   bounds);

            base.PaintBeforeChildren(spriteBatch, bounds);
        }
    }
}