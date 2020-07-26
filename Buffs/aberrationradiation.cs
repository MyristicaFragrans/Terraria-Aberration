
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace aberration.Buffs
{
	public class aberrationradiation : ModBuff
	{
		public override void SetDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
			DisplayName.SetDefault("Aberrant Radiation");
			Description.SetDefault("Rapidly losing life");
		}
		
		
		public override void Update(Player player, ref int buffIndex) {
			aberattionPlayer p = player.GetModPlayer<aberattionPlayer>();
			p.isIrradiated = true;
		}
		
		
	}
}