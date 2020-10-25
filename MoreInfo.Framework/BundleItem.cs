using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using Object = StardewValley.Object;

namespace MoreInfo.Framework
{
    internal class BundleItem
    {
        private static IMonitor _monitor;

        private static readonly List<BundleItem> bundleItems = new List<BundleItem>();

        public BundleItem(IMonitor monitor)
        {
            _monitor = monitor;
        }

        private BundleItem(
            int itemId,
            int itemStack,
            int itemQuality,
            int itemIndex,
            int bundleId,
            int bundleColor,
            string bundleName,
            string bundleLocation)
        {
            ItemId = itemId;
            ItemStack = itemStack;
            ItemQuality = itemQuality;
            ItemIndex = itemIndex;
            BundleId = bundleId;
            BundleColor = bundleColor;
            BundleName = bundleName;
            BundleLocation = bundleLocation;
        }

        private int ItemId { get; }

        private int ItemStack { get; }

        private int ItemQuality { get; }

        private int ItemIndex { get; }

        private int BundleId { get; }

        private int BundleColor { get; }

        private string BundleName { get; }

        private string BundleLocation { get; }

        public static int ItemRequiredForBundle(Item item)
        {
            if (item.ParentSheetIndex < 0 || item.Category == -9)
                return -1;
            if (bundleItems.Count == 0)
                Generate();
            foreach (var bundleItem in bundleItems)
            {
                if (bundleItem.ItemId != item.ParentSheetIndex ||
                    item is Object @object && @object.Quality < bundleItem.ItemQuality)
                    continue;
                var locationFromName = (CommunityCenter) Game1.getLocationFromName("CommunityCenter");
                if (locationFromName != null &&
                    !locationFromName.bundlesDict()[bundleItem.BundleId][bundleItem.ItemIndex] &&
                    bundleItem.ItemStack > 0)
                    return bundleItem.BundleColor;
            }

            return -1;
        }

        private static void Generate()
        {
            foreach (var keyValuePair in Game1.content.Load<Dictionary<string, string>>("Data\\Bundles"))
            {
                var strArray1 = keyValuePair.Key.Split('/');
                var strArray2 = keyValuePair.Value.Split('/');
                var strArray3 = strArray2[2]
                    .Split(' ');
                _monitor.Log($"{strArray2[3]}");
                for (var index = 0;
                    index < strArray3.Length;
                    index += 3)
                    bundleItems.Add(new BundleItem(Convert.ToInt32(strArray3[index]),
                        Convert.ToInt32(strArray3[index + 1]),
                        Convert.ToInt32(strArray3[index + 2]),
                        index / 3,
                        Convert.ToInt32(strArray1[1]),
                        Convert.ToInt32(strArray2[3]),
                        strArray2[0],
                        strArray1[0]));
            }
        }
    }
}