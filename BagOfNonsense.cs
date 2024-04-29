using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Accessory;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    internal partial class BagOfNonsense : Mod
    {
        internal enum MessageType : byte
        {
            CursedSkullSyncedData
        }

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> shockWave = new(ModContent.Request<Effect>("BagOfNonsense/Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value);
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(shockWave, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType messageType = (MessageType)reader.ReadByte();
            switch (messageType)
            {
                case MessageType.CursedSkullSyncedData:
                    byte cursedPlayer = reader.ReadByte();
                    CursedSkullModPlayer oneEquippedSkull = Main.player[cursedPlayer].GetModPlayer<CursedSkullModPlayer>();
                    oneEquippedSkull.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                        oneEquippedSkull.SyncPlayer(-1, whoAmI, false);
                    break;
            }
        }
    }
}