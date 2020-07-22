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
using System.Linq;
//Note, some of these imports may be unneccesary, I honestly have not checked


namespace aberration
{
	public class aberattionPlayer : ModPlayer
	{
		private int counter = 0;
		private int RadiationCheckHeight = 5275;
		private int RadiatedHeight = 3900;
		
		public override void PostUpdate() {			
			
			
			/*
			//Lag Train station
			//Make expensive proccesses happen on ticks at a specific point.
			
			if(counter>10){
				counter=-1;
			}
			counter=counter+1;
			*/
			
			if(player.Center.Y < RadiatedHeight){
				Main.NewText("-radiation due to height-");
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
						Main.NewText("-radiation due to lack of cover-");
					}
				//}
			}
		}
	}
}