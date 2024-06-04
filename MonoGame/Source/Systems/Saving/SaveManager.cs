using System;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame
{
    public class SaveManager
    {
        public static void SaveGame(string dirPath)
        {
            if (Globals.networkMode == NetworkMode.Client)
            {
                return;
            }

            (List<PlayerState>, List<EntityState>, List<ChunkState>) worldState = Globals.world.GetWorldState();

            string json = JsonConvert.SerializeObject(worldState.Item1);

            string playersFolderPath = Path.Combine(dirPath, "players");
            Directory.CreateDirectory(playersFolderPath);

            for (int i = 0; i < worldState.Item1.Count; i++)
            {
                string playerJson = JsonConvert.SerializeObject(worldState.Item1[i]);
                string chunkFilePath = Path.Combine(playersFolderPath, $"player_{worldState.Item1[i].UUID}.json");
                File.WriteAllText(chunkFilePath, playerJson);
            }

            json = JsonConvert.SerializeObject(worldState.Item2);
            File.WriteAllText(dirPath + "entities.json", json);

            string chunksFolderPath = Path.Combine(dirPath, "chunks");
            Directory.CreateDirectory(chunksFolderPath);

            for (int i = 0; i < worldState.Item3.Count; i++)
            {
                string chunkJson = JsonConvert.SerializeObject(worldState.Item3[i]);
                string chunkFilePath = Path.Combine(chunksFolderPath, $"chunk_{worldState.Item3[i].X}_{worldState.Item3[i].Y}.json");
                File.WriteAllText(chunkFilePath, chunkJson);
            }
        }

        public static bool LoadGame(string dirPath)
        {
            if (Directory.Exists(dirPath) && Directory.Exists(dirPath + "players") && Directory.Exists(dirPath + "chunks") && File.Exists(dirPath + "entities.json"))
            {
                List<string> chunksJson = new List<string>();
                string chunksFolderPath = Path.Combine(dirPath, "chunks");
                if (Directory.Exists(chunksFolderPath))
                {
                    string[] chunkFiles = Directory.GetFiles(chunksFolderPath, "*.json");
                    foreach (string chunkFilePath in chunkFiles)
                    {
                        string chunkJson = File.ReadAllText(chunkFilePath);
                        chunksJson.Add(chunkJson);
                    }
                }

                List<string> playersJson = new List<string>();
                string playersFolderPath = Path.Combine(dirPath, "players");
                if (Directory.Exists(playersFolderPath))
                {
                    string[] playerFiles = Directory.GetFiles(playersFolderPath, "*.json");
                    foreach (string playerFilesPath in playerFiles)
                    {
                        string playerJson = File.ReadAllText(playerFilesPath);
                        playersJson.Add(playerJson);
                    }
                }

                string entitiesJson = File.ReadAllText(dirPath + "entities.json");

                List<PlayerState> playerStates = new List<PlayerState>();
                foreach (string playerJson in playersJson)
                {
                    PlayerState playerState = JsonConvert.DeserializeObject<PlayerState>(playerJson);
                    playerStates.Add(playerState);
                }

                List<ChunkState> chunkStates = new List<ChunkState>();
                foreach (string chunkJson in chunksJson)
                {
                    ChunkState chunkState = JsonConvert.DeserializeObject<ChunkState>(chunkJson);
                    chunkStates.Add(chunkState);
                }
                List<EntityState> entityStates = JsonConvert.DeserializeObject<List<EntityState>>(entitiesJson);

                World world = new World(playerStates, entityStates, chunkStates);

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
