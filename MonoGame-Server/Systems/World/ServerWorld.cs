using MonoGame;

namespace MonoGame_Server;

public class ServerWorld
{
    public List<ChunkState>? Chunks;
    public List<PlayerState>? Players;
    public List<EntityState>? Entities;
    private SaveManager SaveManager = new SaveManager();

    public void Initialize()
    {
        (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) worldState = SaveManager.LoadGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");

        Players = worldState.Item1;
        Chunks = worldState.Item2;
        Entities = worldState.Item3;
    }

    public (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) GetWorldState()
    {
        return (Players, Chunks, Entities);
    }

    public PlayerState GetPlayerByUUID(string UUID)
    {
        return Players.FirstOrDefault(x => x.UUID == UUID);
    }
}
