using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MonkeyAllianceBot
{
    public class ConnectionManager
    {
        private Dictionary<ulong, AllianceServer> _serverAllianceChannels { get; }

        public ConnectionManager()
        {
            _serverAllianceChannels = new();
        }

        public void AddExistingServer(string companyName, ulong allianceServerID, ulong allianceChannelID, string avatar, DiscordSocketClient client)
        {
            _serverAllianceChannels.Add(allianceChannelID, new(companyName, allianceServerID, allianceChannelID, avatar, client));
        }

        public void AddNewServer(string companyName, ulong allianceServerID, ulong allianceChannelID, string avatar, DiscordSocketClient client)
        {
            AddExistingServer(companyName, allianceServerID, allianceChannelID, avatar, client);

            foreach (var server in _serverAllianceChannels)
            {
                if (server.Value.AllianceServerID != allianceServerID)
                {
                    server.Value.CreateNewWebhook(companyName, avatar, client);

                    _serverAllianceChannels[allianceChannelID].CreateNewWebhook(server.Value.CompanyName, server.Value.Avatar, client);

                }
            }
        }

        public bool serverAllianceChannelKnown(ulong channelID)
        {
            return _serverAllianceChannels.ContainsKey(channelID);
        }

        public void SendMessageToAllAllianceChannels(ulong origin, string author, string message)
        {
            foreach (KeyValuePair<ulong, AllianceServer> serverAllianceChannel in _serverAllianceChannels)
            {
                string prefix = "**" + author + ":**\n> ";
                if (serverAllianceChannel.Key != origin)
                {
                     serverAllianceChannel.Value[_serverAllianceChannels[origin].CompanyName].SendMessageAsync(prefix + message);
                }
            }

        }

        public void Clear()
        {
            foreach (var server in _serverAllianceChannels)
            {
                server.Value.DeleteAllWebhooks();
            }

            _serverAllianceChannels.Clear();
        }




    }
}
