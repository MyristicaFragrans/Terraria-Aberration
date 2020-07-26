using System.Collections.Generic;
using System.Net.Mail;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.World.Generation;
using Terraria.ID;
using Microsoft.Xna.Framework;
using aberration.datastructures;
using System;
using aberration.Tiles;

namespace aberration {
    class aberrationWorld : ModWorld {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            List<Vector2> majorCaves = new List<Vector2> { };
            // Because world generation is like layering several images ontop of each other, we need to do some steps between the original world generation steps.
            GenPass smooth = tasks.Find(t => t.Name == "Smooth World");
            tasks.RemoveRange(1, tasks.Count - 1);

            tasks.Add(new PassLegacy("aberrant", delegate (GenerationProgress progress) {
                progress.Message = "Starting an Aberrant World";
                Main.worldSurface = Main.maxTilesY * 0.2; //vanilla does this at 0.3, but we like caves
                Main.rockLayer = Main.worldSurface + 100;
                for (int x = 0; x < Main.maxTilesX; x++) {
                    for (int y = (int)Main.rockLayer; y < Main.maxTilesY - 200; y++) {
                        WorldGen.PlaceTile(x, y, TileID.Stone);
                    }
                    progress.Set(x / (float)Main.maxTilesX);
                }

            }));
            bool[,] map = new bool[Main.maxTilesX / 3, Main.maxTilesY / 3];
            tasks.Add(new PassLegacy("tunnels", delegate (GenerationProgress progress) {
                progress.Message = "Planning out small caves";
                for (int x = 0; x < map.GetLength(0); x++) {
                    for (int y = 0; y < map.GetLength(1); y++) {
                        map[x, y] = WorldGen.genRand.NextFloat(0, 1) <= 0.37 ? true : false;
                    }
                }
                progress.Set(0.33f);
                map = minorCave(map);
                progress.Set(0.66f);
                map = minorCave(map);
                progress.Set(1f);
            }));
            tasks.Add(new PassLegacy("tunnelling", delegate (GenerationProgress progress) {
                progress.Message = "Digging out small caves";

                for (int x = 0; x < map.GetLength(0); x++) {
                    for (int y = 0; y < map.GetLength(1); y++) {
                        if (map[x, y] == true) {
                            WorldGen.TileRunner(x*3, y*3, 6, WorldGen.genRand.Next(5,10), -1);
                        }
                    }
                    progress.Set(x / (float)map.GetLength(0));
                }
                for (int x = 0; x < Main.maxTilesX; x++) {
                    WorldGen.TileRunner(x, (int)Main.rockLayer, 10, 10, TileID.Stone, true);
                }
            }));
            
            tasks.Add(new PassLegacy("major", delegate (GenerationProgress progress) {
                progress.Message = "Hiring excavation crews...";
                //between 700 and 1200 Y
                int padding = 500;
                int steps = (int)((Main.maxTilesX - padding * 2) * 0.002); //comes out to about 22 steps on a large world
                int stepSize = (int)(Main.maxTilesX / (float)steps);
                Vector2 last = default;
                for (int i = 0; i < steps; i++) {
                    Vector2 node = new Vector2(500 + i * stepSize, WorldGen.genRand.Next(700, 1200));
                    if (!Equals(last, default(Vector2))) {
                        majorCaves.AddRange(bresenham(last, node));
                    }
                    last = node;
                }
            }));
            tasks.Add(new PassLegacy("drawMajor", delegate (GenerationProgress progress) {
                progress.Message = "Making Big Caves";
                for (int i = 0; i < majorCaves.Count; i++) {
                    if(WorldGen.genRand.Next(3) != 0)
                        WorldGen.TileRunner((int)majorCaves[i].X, (int)majorCaves[i].Y, 50, WorldGen.genRand.Next(40, 80), TileID.Stone, true);
                    progress.Set(i / (float)(majorCaves.Count * 2));
                }
                for (int i = 0; i < majorCaves.Count; i++) {
                    WorldGen.TileRunner((int)majorCaves[i].X, (int)majorCaves[i].Y, 25, WorldGen.genRand.Next(10, 50), -1, true);
                    progress.Set(0.5f + i / ((float)majorCaves.Count * 2));
                }
            }));

            tasks.Add(new PassLegacy("Shinies", delegate (GenerationProgress progress) {
                progress.Message = "Dropping Precious Stones";
                for (int x = 0; x < Main.maxTilesX; x++) {
                    for (int y = (int)Main.rockLayer; y < Main.maxTilesY - 200; y++) {

                        int distanceFromTop = y - (int)Main.rockLayer;
                        if ( WorldGen.genRand.Next(0, (distanceFromTop+100)*100) == 0 ) WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(5, 10), TileType<IonizingMetal>());
                        else if (WorldGen.genRand.Next(0, 2000) == 0) WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(5, 10), TileID.Iron);
                    }
                }
            }));
            tasks.Add(new PassLegacy("placeDirt", delegate (GenerationProgress progress) {
                progress.Message = "Gardening..."; List<Vector2> inner20 = new List<Vector2> { };
                List<Vector2> gardenSites = new List<Vector2> { };
                if (majorCaves.Count <= 21) inner20 = majorCaves;
                else {
                    int center = majorCaves.Count / 2;
                    inner20 = majorCaves.GetRange(center - 10, 20);
                }
                for (int i = 0; i < 20; i++) {
                    if (inner20.Count != 0) {
                        int index = WorldGen.genRand.Next(inner20.Count);
                        Vector2 dropDirt = inner20[index];
                        inner20.RemoveAt(index);

                        while (!Main.tile[(int)dropDirt.X, (int)dropDirt.Y].active()) {
                            dropDirt.Y++;
                        }
                        WorldGen.TileRunner((int)dropDirt.X, (int)dropDirt.Y, WorldGen.genRand.Next(1, 6), WorldGen.genRand.Next(10, 30), TileID.Dirt);
                        gardenSites.Add(dropDirt);
                    }
                    else i = 20;
                }
                List<Vector2> inner25 = new List<Vector2> { };
                inner25 = majorCaves.GetRange(majorCaves.Count / 2 - majorCaves.Count / 8, majorCaves.Count / 4);
                for (int i = 0; i < 50; i++) {
                    if (inner25.Count != 0) {
                        int index = WorldGen.genRand.Next(inner25.Count);
                        Vector2 dropDirt = inner25[index];
                        inner25.RemoveAt(index);

                        while (!Main.tile[(int)dropDirt.X, (int)dropDirt.Y].active()) {
                            dropDirt.Y++;
                        }
                        WorldGen.TileRunner((int)dropDirt.X, (int)dropDirt.Y, 20, WorldGen.genRand.Next(10, 50), 0);
                        gardenSites.Add(dropDirt);
                    }
                    else i = 50;
                }
                for(int i = 0; i < gardenSites.Count; i++) {
                    garden((int)gardenSites[i].X, (int)gardenSites[i].Y, 20);
                }
            }));
            tasks.Add(new PassLegacy("Smoothing", delegate (GenerationProgress progress) {
                progress.Message = "Erosion";
                smooth.Apply(new GenerationProgress()); // I have no clue how Smoothing does it's GenerationProgress so I decided to hold my ears and sing LALALALA
            }));
            tasks.Add(new PassLegacy("Final", delegate (GenerationProgress progress) {
                
                Vector2 spawnpoint = majorCaves[majorCaves.Count / 2];
                while (!Main.tile[(int)spawnpoint.X, (int)spawnpoint.Y].active() || Main.tile[(int)spawnpoint.X, (int)spawnpoint.Y].type == TileID.Trees) {
                    spawnpoint.Y++;
                }
                setRect((int)spawnpoint.X - 7, (int)spawnpoint.Y - 5, 14, 4, forced: true);
                setRectWall((int)spawnpoint.X - 6, (int)spawnpoint.Y - 4, 13, 4, WallID.Wood);
                // ceiling
                setRect((int)spawnpoint.X - 7, (int)spawnpoint.Y - 5, 15, 1, TileID.WoodBlock, true);
                //floor
                setRect((int)spawnpoint.X - 7, (int)spawnpoint.Y, 14, 1, TileID.WoodBlock, true);
                //left wall
                setRect((int)spawnpoint.X - 7, (int)spawnpoint.Y - 4, 1, 4, TileID.WoodenBeam, forced: true);
                //right wall
                setRect((int)spawnpoint.X + 7, (int)spawnpoint.Y - 4, 1, 4, TileID.WoodenBeam, forced: true);
                //torches
                WorldGen.KillTile((int)spawnpoint.X - 4, (int)spawnpoint.Y - 3);
                WorldGen.KillTile((int)spawnpoint.X - 4, (int)spawnpoint.Y - 2);
                WorldGen.PlaceTile((int)spawnpoint.X - 4, (int)spawnpoint.Y - 3, TileType<Tiles.IonizedLantern>());
                WorldGen.KillTile((int)spawnpoint.X + 4, (int)spawnpoint.Y - 3);
                WorldGen.KillTile((int)spawnpoint.X + 4, (int)spawnpoint.Y - 2);
                WorldGen.PlaceTile((int)spawnpoint.X + 4, (int)spawnpoint.Y - 3, TileType<Tiles.IonizedLantern>());

                //finally set spawnpoint
                Main.spawnTileX = (int)spawnpoint.X;
                Main.spawnTileY = (int)spawnpoint.Y - 1;

                //merchant
                NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.Merchant, 1);
            }));
        }
        internal static bool[,] minorCave(bool[,] initial) {
            bool[,] output = new bool[initial.GetLength(0), initial.GetLength(1)];
            for (int x = 0; x < initial.GetLength(0); x++) {
                for (int y = 0; y < initial.GetLength(1); y++) {
                    int neighbors = 0;
                    for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                            if (i == 0 && j == 0) continue;
                            else if (x + i >= initial.GetLength(0) || x + i < 0) neighbors++;
                            else if (y + j >= initial.GetLength(1) || y + j < 0) neighbors++; // assume inaccessable tiles are active
                            else if (initial[x + i, y + j] == true) neighbors++;
                        }
                    }
                    if (neighbors >= 4) output[x, y] = true;
                    if (neighbors <= 3) output[x, y] = false;
                }
            }
            return output;
        }
        internal static void setRect(int x, int y, int w, int h, int type = -1, bool forced = false, int style = 0) {
            for (int cx = x; cx < x + w; cx++) {
                for (int cy = y; cy < y + h; cy++) {
                    if (type == -1)
                        WorldGen.KillTile(cx, cy, noItem: true);
                    else
                        WorldGen.PlaceTile(cx, cy, type, forced: forced, style: style);
                }
            }
        }
        internal static void setRectWall(int x, int y, int w, int h, int type = -1) {
            for (int cx = x; cx < x + w; cx++) {
                for (int cy = y; cy < y + h; cy++) {
                    if (type == -1) {
                        int tries = 50;
                        while (!Main.tile[x, y].active() && tries > 0) {
                            WorldGen.KillWall(cx, cy);
                            tries--;
                        }
                    }
                    else {
                        int tries = 50;
                        while (Main.tile[x, y].type != type && tries > 0) {
                            WorldGen.PlaceWall(cx, cy, type);
                            tries--;
                        }
                    }
                }
            }
        }
        internal static void smoothUnderground() {
            List<Vector2> killBlocks = new List<Vector2> { };
            for (int x = 1; x < Main.maxTilesX - 1; x++) {
                for (int y = 1; y < Main.maxTilesY - 1; y++) {
                    if (Main.tile[x, y].active() && Main.tile[x, y].type == TileID.Stone) {
                        int score = 0;
                        if (Main.tile[x, y - 1].active()) score++;
                        if (Main.tile[x, y + 1].active()) score++;
                        if (Main.tile[x - 1, y].active()) score++;
                        if (Main.tile[x + 1, y].active()) score++;

                        if (score <= 2) killBlocks.Add(new Vector2(x, y));
                    }
                }
            }
            int max = killBlocks.Count;
            while (killBlocks.Count > 0) {
                Vector2 toKill = killBlocks[0];
                killBlocks.RemoveAt(0);
                killBlocks.TrimExcess();
                WorldGen.KillTile((int)toKill.X, (int)toKill.Y);
            }
        }
        internal static void garden(int x, int y, int range) {

            //quick smoothing operation on the dirt
            List<Vector2> killBlocks = new List<Vector2> { };
            for (int gx = x - range; gx < x + range; gx++) {
                for (int gy = y - range; gy < y + range; gy++) {
                    if (Main.tile[x, y].active() && Main.tile[x, y].type == TileID.Dirt) {
                        int score = 0;
                        if (Main.tile[gx, gy - 1].active()) score++;
                        if (Main.tile[gx, gy + 1].active()) score++;
                        if (Main.tile[gx - 1, gy].active()) score++;
                        if (Main.tile[gx + 1, gy].active()) score++;

                        if (score <= 3) killBlocks.Add(new Vector2(x, y)); //we are more aggressive on the score here than on smoothUnderground because dirt is weird
                    }
                }
            }
            while (killBlocks.Count > 0) {
                Vector2 toKill = killBlocks[0];
                killBlocks.RemoveAt(0);
                killBlocks.TrimExcess();
                WorldGen.KillTile((int)toKill.X, (int)toKill.Y);
            }
            for (int gx = x - range; gx < x + range; gx++) {
                for (int gy = y - range; gy < y + range; gy++) {
                    Tile selected = Main.tile[gx, gy];
                    if (selected.active() && selected.type == TileID.Dirt) {
                        if (!Main.tile[gx + 1, gy].active() || !Main.tile[gx - 1, gy].active() || !Main.tile[gx, gy + 1].active() || !Main.tile[gx, gy - 1].active()) {
                            Main.tile[gx, gy].type = TileID.Grass;
                            if (!Main.tile[gx, gy - 1].active()) {
                                WorldGen.GrowTree(gx, gy);
                                if (!Main.tile[gx, gy - 1].active()) {
                                    if (WorldGen.genRand.Next(12) == 0 && IonizedLantern.canPlacePublic(gx,gy-1)) {
                                        try {
                                            WorldGen.PlaceTile(gx, gy - 1, TileType<Tiles.IonizedLantern>(), forced: true);
                                        } catch {
                                            WorldGen.PlaceTile(gx, gy - 1, 3, style: WorldGen.genRand.Next(1, 42)); //tall grass
                                        }
                                    } else {
                                        WorldGen.PlaceTile(gx, gy - 1, 3, style: WorldGen.genRand.Next(1, 42)); //tall grass
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        internal static List<Vector2> bresenham(Vector2 last, Vector2 node) {
            List<Vector2> returns = new List<Vector2> { };
            float deltax = node.X - last.X;
            float deltay = node.Y - last.Y;

            int yi = 1;
            if (deltay < 0) {
                yi = -1;
                deltay = -deltay;
            }
            float error = 2 * deltay - deltax;
            int y = (int)last.Y;

            for (int x = (int)last.X; x < node.X; x++) {
                returns.Add(new Vector2(x, y));
                if (error > 0) {
                    y = y + yi;
                    error = error - 2 * deltax;
                }
                error = error + 2 * deltay;
            }
            return returns;
        }
    }
}
