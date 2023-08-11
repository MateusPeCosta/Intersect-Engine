using Intersect.Network.Packets.Client;

namespace Intersect.Server.Networking
{
    internal sealed partial class PacketHandler
    {
        public void HandlePacket(Client client, RunningPacket packet)
        {
            var player = client.Entity;
            if (player == null)
            {
                return;
            }

            player.IsRunning = !player.IsRunning;
        }
    }
}
