namespace MonoGame_Server.Systems.Server.MessageHandlers;

public interface IServerMessageHandler<T>
{
    public abstract void Validate(T message);
    public abstract void Execute(T message);
}
