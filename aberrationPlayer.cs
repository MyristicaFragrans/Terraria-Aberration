using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using aberration.Dusts;
using System.Linq;
//Note, some of these imports may be unneccesary, I honestly have not checked


namespace aberration
{
	public class aberattionPlayer : ModPlayer
	{
		public bool isIrradiated;
		private int counter = 0;
		
		
		public override void ResetEffects() {
			isIrradiated = false;
		}
		public override void UpdateDead() {
			isIrradiated = false;
		}
		
		public override void UpdateBadLifeRegen() {
			if (isIrradiated) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 200;
			}
		}
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (isIrradiated) {
				if (Main.rand.NextBool(4) && drawInfo.shadow == 0f) {
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, DustType<RadiationDust>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				r *= 0.1f;
				g *= 1f;
				b *= 0.1f;
				fullBright = true;
			}
		}
		public override void PostUpdate() {			
			int RadiationCheckHeight = 211/48 * Main.maxTilesY; //Against how this seems, this is not in tile coordinates, it is in world coordinates
			int RadiatedHeight = (13/4) * Main.maxTilesY;//These numbers ain't perfect: Probally should use world gen numbers at some point
			/*
			//Lag Train station
			//Make expensive proccesses happen on ticks at a specific point.
			
			if(counter>10){
				counter=-1;
			}
			counter=counter+1;
			*/
			
			if(player.Center.Y < RadiatedHeight){
				player.AddBuff(mod.BuffType("aberrationradiation"), 1);
			}else if(player.Center.Y < RadiationCheckHeight){
				//if(counter==0){ // If the lag train arrives
					
					//Setup a Ray
					Vector2 movePos = new Vector2(player.Center.X,player.Center.Y);
					int spread = 3+Main.rand.Next(3);//How spread apart should the ray be, this is a slope.
					bool spreadDir = Main.rand.NextBool();
					float pierce = 5.0f;// How many blocks can we pierce through, water counts as 4 blocks
					int walkpoint; //For scope reasons
					
					
					//Follow the Ray
					for(walkpoint = 0; pierce > 0 && movePos.Y > RadiatedHeight; walkpoint++){
						if(Main.tile[(int) (movePos.X/16), (int) (movePos.Y/16)].active()){
							pierce=pierce-1;
							//Main.NewText("pierced target");
						}
						
						if(walkpoint == spread){//Only move left/right occasionally based on spread;
							walkpoint = 0;//Reset slope counter
							
							if(spreadDir){//Move based on direction
								movePos = new Vector2((int) movePos.X+16,(int) movePos.Y);
							}else{
								movePos = new Vector2((int) movePos.X-16,(int) movePos.Y);
							}
						}else{
							movePos = new Vector2((int) movePos.X,(int) movePos.Y-16);
						}
					}
					//Main.NewText(pierce);
					if(pierce != 0 || movePos.Y < RadiatedHeight){
						player.AddBuff(mod.BuffType("aberrationradiation"), 1);
					}
				//}
			}
		}
	}
}