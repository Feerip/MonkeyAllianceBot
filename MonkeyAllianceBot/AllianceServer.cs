using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.Rest;
using Discord.Webhook;
using Discord.WebSocket;
using System.IO;
using System.Threading;

namespace MonkeyAllianceBot
{
    class AllianceServer
    {
        public string CompanyName { get; private set; }
        
        public ulong AllianceServerID { get; private set; }
        public ulong AllianceChannelID { get; private set; }
        public string Avatar { get; private set; }
        
        private Dictionary<string, DiscordWebhookClient> Webhooks { get; set; }

        public AllianceServer(string companyName, ulong allianceServerID, ulong allianceChannelID, string avatar, DiscordSocketClient client)
        {
            CompanyName = companyName;
            AllianceServerID = allianceServerID;
            AllianceChannelID = allianceChannelID;
            Avatar = avatar;
            
            RefreshServerStatus(client);
        }

        public void RefreshServerStatus(DiscordSocketClient client)
        {
            Webhooks = new();


            var hooks = client.GetGuild(AllianceServerID).GetTextChannel(AllianceChannelID).GetWebhooksAsync().Result;
            foreach (RestWebhook hook in hooks)
            {
                AddExistingWehbook(hook.Name, new(hook));
            }
        }

        void AddExistingWehbook(string companyName, DiscordWebhookClient hook)
        {
            Webhooks.Add(companyName, hook);
        }

        
        public async void CreateNewWebhook(string companyName, string avatar, DiscordSocketClient client)
        {
            //Need to avoid rate limits
            Thread.Sleep(2000);

            FileStream avatarStream = File.Open(avatar, FileMode.Open, FileAccess.Read);
            SocketTextChannel socketTextChannel = client.GetGuild(AllianceServerID).GetTextChannel(AllianceChannelID);

            DiscordWebhookClient newWebhook = new(await socketTextChannel.CreateWebhookAsync(companyName, avatar: avatarStream));

            Webhooks.Add(companyName, newWebhook);

            avatarStream.Close();

            

        }
        public void DeleteAllWebhooks()
        {
            foreach (var hook in Webhooks)
            {
                hook.Value.DeleteWebhookAsync();
            }
            Webhooks.Clear();
        }

        public DiscordWebhookClient this[string companyName]
        { 
            get { return Webhooks[companyName]; }
        }



    }
}
