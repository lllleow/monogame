namespace MonoGame_Common.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NetworkMessageAttribute : Attribute
{
    public NetworkMessageAttribute(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
}