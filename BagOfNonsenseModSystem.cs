using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    internal class BagOfNonsenseModSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup evilmaterail = new(() => "Any evil material", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup("Any evil material", evilmaterail);
        }
    }
}