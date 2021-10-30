using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;

namespace MonkeyAllianceBot
{
    public class ConnectionManager
    {
        private Dictionary<ulong, AllianceServer> _serverAllianceChannels { get; }

        public ConnectionManager()
        {
            _serverAllianceChannels = new();
        }

        public void AddExistingServer(string companyName, ulong allianceServerID, ulong allianceChannelID, string avatar, DiscordSocketClient client, Color? companyColor = null)
        {
            _serverAllianceChannels.Add(allianceChannelID, new(companyName, allianceServerID, allianceChannelID, avatar, client, companyColor));
        }

        public void AddNewServer(string companyName, ulong allianceServerID, ulong allianceChannelID, string avatar, DiscordSocketClient client, Color? companyColor = null)
        {
            AddExistingServer(companyName, allianceServerID, allianceChannelID, avatar, client, companyColor);

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

        public void SendMessageToAllAllianceChannels(ulong origin, SocketCommandContext context)
        {
            foreach (KeyValuePair<ulong, AllianceServer> serverAllianceChannel in _serverAllianceChannels)
            {
                //    string prefix = "**" + author + ":**\n> ";

                //    if (serverAllianceChannel.Key != origin)
                //    {
                //         serverAllianceChannel.Value[_serverAllianceChannels[origin].CompanyName].SendMessageAsync(prefix + message);
                //    }
                EmbedBuilder builder = new EmbedBuilder()
                    .WithAuthor(context.Guild.GetUser(context.User.Id).Nickname, iconUrl: context.User.GetAvatarUrl())
                    //.WithTitle(context.User.Mention)
                    .WithDescription(context.Message.Content)
                    .WithFooter(context.User.Mention)
                    //.AddField("\u200B", context.User.Mention)
                    .WithColor(_serverAllianceChannels[origin].CompanyColor)

                    ;
                if (context.Message.Attachments.Count > 0)
                {
                    builder.WithImageUrl(context.Message.Attachments.ToList().First().Url);
                }
                Embed emb = builder.Build();

                if (serverAllianceChannel.Key != origin)
                {
                    serverAllianceChannel.Value[_serverAllianceChannels[origin].CompanyName].SendMessageAsync(embeds: new[] { emb });
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
