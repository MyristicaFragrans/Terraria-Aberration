using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace aberration.Dusts
{
	public class RadiationDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity.Y = Main.rand.Next(-5, 5) * 0.1f;
			dust.velocity.X = Main.rand.Next(-5, 5) * 0.1f;
			dust.scale *= 0.7f;
		}

		public override bool MidUpdate(Dust dust) {
			dust.velocity.Y *= 0.95f;
			dust.velocity.X *= 0.95f;

			if (dust.noLight) {
				return false;
			}

			float strength = dust.scale * 1.4f;
			if (strength > 1f) {
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 0.1f * strength, (0.8f+(Main.rand.Next(-30, 15)*0.01f)) * strength, 0.1f * strength);
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) 
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
}