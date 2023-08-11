using Intersect.Network.Packets.Client;

namespace Intersect.Client.Networking
{
    public static partial class PacketSender
    {
        public static void SendRunningPacket()
        {
            Network.SendPacket(new RunningPacket());
        }
    }
}
