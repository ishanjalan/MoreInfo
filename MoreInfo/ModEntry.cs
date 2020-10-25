using System;
using Harmony;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreInfo.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using Context = MoreInfo.Framework.Context;

namespace MoreInfo
{
    public class ModEntry : Mod
    {
        private Texture2D _emptyTex;

        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create(ModManifest.UniqueID);
            harmony.Patch(AccessTools.Method(
                    typeof(StardewValley.Object),
                    nameof(StardewValley.Object.drawInMenu),
                    new Type[8]
                    {
                        typeof(SpriteBatch),
                        typeof(Vector2),
                        typeof(float),
                        typeof(float),
                        typeof(float),
                        typeof(StackDrawType),
                        typeof(Color),
                        typeof(bool)
                    }),
                null,
                new HarmonyMethod(typeof(ObjectDrawInMenuPatch),
                    nameof(ObjectDrawInMenuPatch.drawInMenu_Postfix)));
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.World.ObjectListChanged += OnObjectListChanged;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
            _emptyTex = new Texture2D(Game1.graphics.GraphicsDevice,
                1,
                1);
            var white = new Color[1]
            {
                Color.White
            };
            _emptyTex.SetData(white);
        }

        private static void OnDayStarted(object sender,
            DayStartedEventArgs e)
        {
            Context.updateOwnedItems();
        }

        private static void OnObjectListChanged(object sender,
            ObjectListChangedEventArgs e)
        {
            Context.updateOwnedItems();
        }

        private static void OnInventoryChanged(object sender,
            InventoryChangedEventArgs e)
        {
            Context.updateOwnedItems();
        }

        private static void OnOneSecondUpdateTicked(object sender,
            OneSecondUpdateTickedEventArgs e)
        {
            lock (Context.QuestItems)
            {
                Context.QuestItems.Clear();
                using (var enumerator =
                    Game1.player.questLog.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        switch (current)
                        {
                            case ItemDeliveryQuest itemDeliveryQuest:
                                Context.QuestItems.Add(itemDeliveryQuest.item.Value);
                                break;
                            case ResourceCollectionQuest resourceCollectionQuest:
                                Context.QuestItems.Add(
                                    resourceCollectionQuest.resource.Value);
                                break;
                            case FishingQuest fishingQuest:
                                Context.QuestItems.Add(fishingQuest.whichFish.Value);
                                break;
                        }
                    }
                }
            }
        }
    }
}