using System;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;

namespace MonoGame;

public class NetworkClient
{
    EventBasedNetListener listener;
    public NetManager client;

    public void InitializeClient()
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);

        client.Start();
        client.Connect("localhost", 9050, "monogame");

        listener.PeerConnectedEvent += peer =>
        {
            SendUUID();
        };

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
        {
            Console.WriteLine("We got: {0}", dataReader.GetString(100));
            dataReader.Recycle();
        };
    }

    public void SendUUID()
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put((byte)1);
        writer.Put(Globals.world.Players.First().UUID);
        Globals.networkManager.GetClient()?.SendMessage(writer);
    }

    public void SendMessage(NetDataWriter reader)
    {
        client.FirstPeer.Send(reader, DeliveryMethod.ReliableOrdered);
    }

    public void Update()
    {
        client.PollEvents();
    }

    public void Stop()
    {
        client.Stop();
    }
}
