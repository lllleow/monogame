using System;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MonoGame
{
    public class SaveManager
    {
        public static void SaveGame(string dirPath)
        {
            (PlayerState, List<EntityState>, List<ChunkState>) worldState = Globals.world.GetWorldState();

            string json = JsonConvert.SerializeObject(worldState.Item1);
            File.WriteAllText(dirPath + "player.json", json);

            json = JsonConvert.SerializeObject(worldState.Item2);
            File.WriteAllText(dirPath + "entities.json", json);

            json = JsonConvert.SerializeObject(worldState.Item3);
            File.WriteAllText(dirPath + "chunks.json", json);
        }

        public static bool LoadGame(string dirPath)
        {
            if (Directory.Exists(dirPath) && File.Exists(dirPath + "player.json") && File.Exists(dirPath + "chunks.json") && File.Exists(dirPath + "entities.json"))
            {
                string playerJson = File.ReadAllText(dirPath + "player.json");
                string chunksJson = File.ReadAllText(dirPath + "chunks.json");
                string entitiesJson = File.ReadAllText(dirPath + "entities.json");

                PlayerState playerState = JsonConvert.DeserializeObject<PlayerState>(playerJson);
                List<ChunkState> chunkStates = JsonConvert.DeserializeObject<List<ChunkState>>(chunksJson);
                List<EntityState> entityStates = JsonConvert.DeserializeObject<List<EntityState>>(entitiesJson);

                World world = new World(playerState, entityStates, chunkStates);

                Globals.world = world;
                return true;
            }
            else
            {
                Console.WriteLine("Save file not found.");
                return false;
            }
        }
    }
}
