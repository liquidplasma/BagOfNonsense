using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class CursedSkullNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && player.GetModPlayer<CursedSkullModPlayer>().isActive && npc.CanBeChasedBy() && !npc.boss && npc.Center.DistanceSQ(player.Center) <= 3000 * 3000)
                {
                    npc.lifeMax *= 10;
                    npc.life = npc.lifeMax;
                    npc.value *= 10;
                    npc.netUpdate = true;
                }
            }
        }
    }

    public class CursedSkullModPlayer : ModPlayer
    {
        public bool isActive;

        public override void ResetEffects()
        {
            isActive = false;
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            CursedSkullModPlayer clone = (CursedSkullModPlayer)targetCopy;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            clone.isActive = isActive;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BagOfNonsense.MessageType.CursedSkullSyncedData);
            packet.Write((byte)Player.whoAmI);
            packet.Write(isActive);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            CursedSkullModPlayer clone = (CursedSkullModPlayer)clientPlayer;
            if (isActive != clone.isActive)
            {
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            }
            base.SendClientChanges(clientPlayer);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            isActive = reader.ReadBoolean();
        }
    }

    public class CursedSkull : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CursedSkullModPlayer>().isActive = true;
        }
    }
}