using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Objects;

namespace MoreInfo.Framework
{
    public static class Context
    {
        private static Texture2D _junimoNoteText;
        private static readonly Dictionary<int, int> ownedItems = new Dictionary<int, int>();
        public static List<int> QuestItems = new List<int>();

        internal static Texture2D JunimoNoteText =>
            _junimoNoteText ??
            (_junimoNoteText =
                Game1.content.Load<Texture2D>("LooseSprites\\JunimoNote"));

        public static void updateOwnedItems()
        {
            ownedItems.Clear();
            var enumerator = Game1.player.Items.GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    if (!(current is Object @object) ||
                        @object is Furniture ||
                        @object.bigCraftable.Value)
                        continue;
                    if (ownedItems.ContainsKey(current.ParentSheetIndex))
                        ownedItems[current.ParentSheetIndex] += current.Stack;
                    else
                        ownedItems.Add(current.ParentSheetIndex,
                            current.Stack);
                }
            }

            using (var enumerator1 = Game1.locations.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                    using (var enumerator2 =
                        enumerator1.Current?.objects.Values
                            .Where(x => x is Chest)
                            .GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var current1 = (Chest) enumerator2.Current;
                            if (!current1.playerChest.Value)
                                continue;
                            using (var enumerator3 =
                                current1.items.GetEnumerator())
                            {
                                while (enumerator3.MoveNext())
                                {
                                    var current2 = enumerator3.Current;
                                    if (!(current2 is Object @object) ||
                                        @object is Furniture ||
                                        @object.bigCraftable.Value)
                                        continue;
                                    if (ownedItems.ContainsKey(current2.ParentSheetIndex))
                                        ownedItems[current2.ParentSheetIndex] += current2.Stack;
                                    else
                                        ownedItems.Add(current2.ParentSheetIndex,
                                            current2.Stack);
                                }
                            }
                        }
                    }
            }
        }

        internal static bool OwnsItemQuantity(int itemIndex,
            int quantity)
        {
            return ownedItems.ContainsKey(itemIndex) && ownedItems[itemIndex] >= quantity;
        }

        /*internal static int RemoveItemFrom(IList<Item> items, int itemIndex, int quantity)
        {
            var val2 = quantity;
            for (var index = items.Count - 1; index >= 0; --index)
            {
                if (items[index] == null || !(items[index] is Object @object) || @object.bigCraftable.Value ||
                    @object.ParentSheetIndex != itemIndex && @object.Category != itemIndex) continue;
                var stack = items[index].Stack;
                int num;
                items[index].set_Stack(num = stack - val2);
                val2 = -num;
                if (items[index].Stack <= 0)
                    items[index] = null;
                updateOwnedItems();
                if (val2 <= 0)
                    return 0;
            }
            return Math.Max(0, val2);
        }*/
    }
}