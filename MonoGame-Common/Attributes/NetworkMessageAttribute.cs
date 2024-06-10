namespace MonoGame_Common.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NetworkMessageAttribute : Attribute
{
    public int Id { get; private set; }

    public NetworkMessageAttribute(int id)
    {
        Id = id;
    }
}