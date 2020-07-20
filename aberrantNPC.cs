using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace aberration {
    class aberrantNPC : GlobalNPC {
        public override void GetChat(NPC npc, ref string chat) {
            List<string> say = new List<string> { chat };
            if (npc.type == NPCID.Merchant) {
                say.Add("You certainly get us into sticky situations.");
                say.Add("Is the guide here yet? He is usually here before me.");
                say.Add("How is anyone supposed to buy my stuff!?");
            } else if(npc.type == NPCID.Guide) {
                say.Add("Guides are praised for their knowledge. This place is new.");
                say.Add("And I thought hell was a nightmare.");
                say.Add("I've been poking around. The plants here are stronger than usual.");
                say.Add("This place shouldn't exist.");
            } else if(npc.type == NPCID.Dryad) {
                say.Add("I recognise this place. Except I don't. Maybe I do.");
                say.Add("I want to leave this place. How did I get here?");
            } else if(npc.type == NPCID.Stylist) {
                say.Add("It's too damp in here. It's ruining my hair.");
                say.Add("Why can't I just be a barber?");
                say.Add("I have seen a lot of caves. This is my least favorite.");
            } else if(npc.type == NPCID.SkeletonMerchant) {
                say.Add("I finally feel at home here.");
                say.Add("The surface is fantastic! It is so warm.");
                say.Add("Caves caves caves caves caves...");
            }
            chat = say[WorldGen.genRand.Next(say.Count)];
        }
    }
}
