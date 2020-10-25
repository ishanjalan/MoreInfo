using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace MoreInfo.Framework
{
    public class ObjectDrawInMenuPatch
    {
        public static void drawInMenu_Postfix(
            Object instance,
            SpriteBatch spriteBatch,
            Vector2 location,
            float scaleSize,
            float transparency,
            float layerDepth,
            Color color,
            bool drawShadow)
        {
            var num = BundleItem.ItemRequiredForBundle(instance);
            if (num > -1)
                Utility.drawWithShadow(spriteBatch,
                    Context.JunimoNoteText,
                    Vector2.Add(location,
                        new Vector2(6f,
                            4f)),
                    new Rectangle(num * 256 % 512,
                        244 + num * 256 / 512 * 16,
                        16,
                        16),
                    Color.Multiply(color,
                        transparency),
                    0.0f,
                    new Vector2(4f,
                        4f),
                    1.8f * scaleSize,
                    false,
                    layerDepth,
                    1,
                    2,
                    0.6f);
            if (!Context.QuestItems.Contains(instance.ParentSheetIndex))
                return;
            Utility.drawWithShadow(spriteBatch,
                Game1.mouseCursors,
                Vector2.Add(location,
                    new Vector2(56f,
                        4f)),
                new Rectangle(403,
                    496,
                    5,
                    14),
                Color.Multiply(color,
                    transparency),
                0.0f,
                new Vector2(4f,
                    4f),
                1.8f * scaleSize,
                false,
                layerDepth,
                1,
                2,
                0.6f);
        }
    }
}