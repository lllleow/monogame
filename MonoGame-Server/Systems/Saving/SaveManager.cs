﻿using System;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MonoGame_Server;

namespace MonoGame
{
    public class SaveManager
    {
        public static void SaveGame(string dirPath)
        {
            (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) worldState = ServerWorld.Instance.GetWorldState();

            string json = JsonConvert.SerializeObject(worldState.Item1);

            string playersFolderPath = Path.Combine(dirPath, "players");
            Directory.CreateDirectory(playersFolderPath);

            for (int i = 0; i < worldState.Item1?.Count; i++)
            {
                string playerJson = JsonConvert.SerializeObject(worldState.Item1[i]);
                string chunkFilePath = Path.Combine(playersFolderPath, $"player_{worldState.Item1[i].UUID}.json");
                File.WriteAllText(chunkFilePath, playerJson);
            }

            string chunksFolderPath = Path.Combine(dirPath, "chunks");
            Directory.CreateDirectory(chunksFolderPath);

            for (int i = 0; i < worldState.Item2?.Count; i++)
            {
                string chunkJson = JsonConvert.SerializeObject(worldState.Item3?[i]);
                string chunkFilePath = Path.Combine(chunksFolderPath, $"chunk_{worldState.Item2?[i].X}_{worldState.Item2?[i].Y}.json");
                File.WriteAllText(chunkFilePath, chunkJson);
            }

            json = JsonConvert.SerializeObject(worldState.Item3);
            File.WriteAllText(dirPath + "entities.json", json);
        }

        public (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) LoadGame(string dirPath)
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
                    PlayerState? playerState = JsonConvert.DeserializeObject<PlayerState>(playerJson);
                    if (playerState != null)
                    {
                        playerStates.Add(playerState);
                    }
                }

                List<ChunkState> chunkStates = new List<ChunkState>();
                List<EntityState>? entityStates = JsonConvert.DeserializeObject<List<EntityState>>(entitiesJson ?? string.Empty);

                return (playerStates, chunkStates, entityStates);
            }
            else
            {
                Console.WriteLine("Save file not found.");
                return (null, null, null);
            }
        }
    }
}
