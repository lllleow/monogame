using MonoGame_Common.States;
using MonoGame_Server.Systems.Server;
using Newtonsoft.Json;

namespace MonoGame_Server.Systems.Saving;

public class SaveManager
{
    public static string SaveLocation { get; set; } = @"C:\Users\Leonardo\Documents\Repositories\monogame\Save\";

    public static void SaveGame()
    {
        var dirPath = SaveLocation;
        var (Players, Chunks, Entities) = NetworkServer.Instance.ServerWorld.GetWorldState();
        var playersFolderPath = Path.Combine(dirPath, "players");
        _ = Directory.CreateDirectory(playersFolderPath);

        for (var i = 0; i < Players?.Count; i++)
        {
            var playerJson = Serialize(Players[i]);
            var chunkFilePath = Path.Combine(playersFolderPath, $"player_{Players[i].UUID}.json");
            try
            {
                File.WriteAllText(chunkFilePath, playerJson);
            }
            catch (IOException)
            {
                // Handle the exception, e.g., retry or wait for the file to become available.
            }
        }

        var chunksFolderPath = Path.Combine(dirPath, "chunks");
        _ = Directory.CreateDirectory(chunksFolderPath);

        for (var i = 0; i < Chunks?.Count; i++)
        {
            var chunkJson = Serialize(Chunks?[i]);
            var chunkFilePath = Path.Combine(chunksFolderPath, $"chunk_{Chunks?[i].X}_{Chunks?[i].Y}.json");

            try
            {
                File.WriteAllText(chunkFilePath, chunkJson);
            }
            catch (IOException)
            {
                // Handle the exception, e.g., retry or wait for the file to become available.
            }
        }

        var json = Serialize(Entities);

        try
        {
            File.WriteAllText(dirPath + "entities.json", json);
        }
        catch (IOException)
        {
            // Handle the exception, e.g., retry or wait for the file to become available.
        }
    }

    public static (List<PlayerState>? Players, List<ChunkState>? Chunks, List<EntityState>? Entities) LoadGame()
    {
        var dirPath = SaveLocation;
        if (Directory.Exists(dirPath) && Directory.Exists(dirPath + "players") &&
            Directory.Exists(dirPath + "chunks") && File.Exists(dirPath + "entities.json"))
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
                var chunkState = Deserialize<ChunkState>(chunkJson);
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
                var playerState = Deserialize<PlayerState>(playerJson);
                if (playerState != null)
                {
                    playerStates.Add(playerState);
                }
            }

            // Entities
            var entitiesJson = File.ReadAllText(dirPath + "entities.json");
            var entityStates = Deserialize<List<EntityState>>(entitiesJson ?? string.Empty);

            return (playerStates, chunkStates, entityStates);
        }

        Console.WriteLine("Save file not found.");
        return (null, null, null);
    }

    private static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
    }

    private static string Serialize(object? obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
    }
}