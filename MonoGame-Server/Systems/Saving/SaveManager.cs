using MonoGame_Server.Systems.Server;
using MonoGame.Source.WorldNamespace.WorldStates;
using Newtonsoft.Json;

namespace MonoGame_Server.Systems.Saving
{
    public class SaveManager
    {
        public static void SaveGame(string dirPath)
        {
            var worldState = NetworkServer.Instance.ServerWorld.GetWorldState();
            var playersFolderPath = Path.Combine(dirPath, "players");
            _ = Directory.CreateDirectory(playersFolderPath);

            for (var i = 0; i < worldState.Players?.Count; i++)
            {
                var playerJson = JsonConvert.SerializeObject(worldState.Players[i]);
                var chunkFilePath = Path.Combine(playersFolderPath, $"player_{worldState.Players[i].UUID}.json");
                File.WriteAllText(chunkFilePath, playerJson);
            }

            var chunksFolderPath = Path.Combine(dirPath, "chunks");
            _ = Directory.CreateDirectory(chunksFolderPath);

            for (var i = 0; i < worldState.Chunks?.Count; i++)
            {
                var chunkJson = JsonConvert.SerializeObject(worldState.Chunks?[i]);
                var chunkFilePath = Path.Combine(chunksFolderPath, $"chunk_{worldState.Chunks?[i].X}_{worldState.Chunks?[i].Y}.json");
                File.WriteAllText(chunkFilePath, chunkJson);
            }

            var json = JsonConvert.SerializeObject(worldState.Entities);
            File.WriteAllText(dirPath + "entities.json", json);
        }

        public (List<PlayerState>? Players, List<ChunkState>? Chunks, List<EntityState>? Entities) LoadGame(string dirPath)
        {
            if (Directory.Exists(dirPath) && Directory.Exists(dirPath + "players") && Directory.Exists(dirPath + "chunks") && File.Exists(dirPath + "entities.json"))
            {
                // Chunks
                List<string> chunksJson = [];
                var chunksFolderPath = Path.Combine(dirPath, "chunks");
                if (Directory.Exists(chunksFolderPath))
                {
                    var chunkFiles = Directory.GetFiles(chunksFolderPath, "*.json");
                    foreach (var chunkFilePath in chunkFiles)
                    {
                        var chunkJson = File.ReadAllText(chunkFilePath);
                        chunksJson.Add(chunkJson);
                    }
                }

                List<ChunkState> chunkStates = [];
                foreach (var chunkJson in chunksJson)
                {
                    var chunkState = JsonConvert.DeserializeObject<ChunkState>(chunkJson);
                    if (chunkState != null)
                    {
                        chunkStates.Add(chunkState);
                    }
                }

                // Players
                List<string> playersJson = [];
                var playersFolderPath = Path.Combine(dirPath, "players");
                if (Directory.Exists(playersFolderPath))
                {
                    var playerFiles = Directory.GetFiles(playersFolderPath, "*.json");
                    foreach (var playerFilesPath in playerFiles)
                    {
                        var playerJson = File.ReadAllText(playerFilesPath);
                        playersJson.Add(playerJson);
                    }
                }

                List<PlayerState> playerStates = [];
                foreach (var playerJson in playersJson)
                {
                    var playerState = JsonConvert.DeserializeObject<PlayerState>(playerJson);
                    if (playerState != null)
                    {
                        playerStates.Add(playerState);
                    }
                }

                // Entities
                var entitiesJson = File.ReadAllText(dirPath + "entities.json");
                var entityStates = JsonConvert.DeserializeObject<List<EntityState>>(entitiesJson ?? string.Empty);

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
