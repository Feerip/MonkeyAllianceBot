
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Discord.Webhook;
using MonkeyAllianceBot;
using System.IO;


//902080736197242901
//https://discord.com/api/webhooks/902080760809410600/YrumnSHqxxIylTWmF7BNhqG6p85ttaunlf_5GUbB1Gqh10KP-HM07eLNn5GRqnX2XvOw

//902077849513652234
//https://discord.com/api/webhooks/902077884255064074/klzrLvwAuU1ciC2qyOXDVmv55MVP8BHdAPlKEtVKVws56I0TRgHQ8dF7V8k9fU56nULW


namespace MonkeyAllianceBot.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly ulong server1channel = 902080736197242901;
        private readonly string server1_2webhookurl = "https://discord.com/api/webhooks/902080760809410600/YrumnSHqxxIylTWmF7BNhqG6p85ttaunlf_5GUbB1Gqh10KP-HM07eLNn5GRqnX2XvOw";
        private readonly string server1_3webhookurl = "https://discord.com/api/webhooks/902088732214177834/7KWKPHvA9mPcF6OroARM1wGzw1ia3Tgo8QqDZHKXWtmyvHJO1yjqqqwLyxz4kAiKSn5G";
        private readonly ulong server2channel = 902077849513652234;
        private readonly string server2_1webhookurl = "https://discord.com/api/webhooks/902077884255064074/klzrLvwAuU1ciC2qyOXDVmv55MVP8BHdAPlKEtVKVws56I0TRgHQ8dF7V8k9fU56nULW";
        private readonly string server2_3webhookurl = "https://discord.com/api/webhooks/902089232695312454/bZP0_SUwx-YsXFT2uGR5tTbLX_NCZ1YFwc2vYvenKrZ0KeH8Y8KRezdi7HxgTtHEAHhQ";
        private readonly ulong server3channel = 902087868258877491;
        private readonly string server3_1webhookurl = "https://discord.com/api/webhooks/902088573627535370/JRAiO9OyKrL4F3BOQWZa7ZrHJPkRMEvfCeVCkjumZgX3sqpsH0VGP8x0x9PwXjL7YSqv";
        private readonly string server3_2webhookurl = "https://discord.com/api/webhooks/902087917483196416/fBFikdw5kcfNe2B4yIF28RQUhbkG3p7oqf3YNn6KyQ-iucEWXDExdQ2Wjpr0rUKTlw-v";

        private DiscordWebhookClient server1_2webhook;
        private DiscordWebhookClient server1_3webhook;
        private DiscordWebhookClient server2_1webhook;
        private DiscordWebhookClient server2_3webhook;
        private DiscordWebhookClient server3_1webhook;
        private DiscordWebhookClient server3_2webhook;

        ConnectionManager connectionManager = new();


        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
            //server1_2webhook = new DiscordWebhookClient(server1_2webhookurl);
            //server1_3webhook = new DiscordWebhookClient(server1_3webhookurl);
            //server2_1webhook = new DiscordWebhookClient(server2_1webhookurl);
            //server2_3webhook = new DiscordWebhookClient(server2_3webhookurl);
            //server3_1webhook = new DiscordWebhookClient(server3_1webhookurl);
            //server3_2webhook = new DiscordWebhookClient(server3_2webhookurl);

        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;

            _service.CommandExecuted += OnCommandExecuted;
            _client.Ready += SetStatusAsync;
            _client.Ready += _client_Ready;


            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

        }

        private async Task SetStatusAsync()
        {
#if FRUITWARSMODE
           //await _client.SetGameAsync($"Fruit Wars | @FruitBot help", null, ActivityType.Playing);
#else
            //await _client.SetGameAsync($"@FruitBot help", null, ActivityType.Listening);
#endif
        }

        private async Task _client_Ready()
        {
            // Watchdog event to kill the entire program when the Discord.net API glitches out and disconnects. 
            // Calling powershell script will detect the exit and restart it indefinitely.
            // Issues: Calling Environment.Exit(1) to signal that the script should restart is not working properly. Program always exits with exit code 0.
            // Until that's resolved, the powershell script needs to always restart it, but once we get this working it can restart on exit code 1 and 
            //  stay closed on exit code 0.

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(CheckConnection);
            timer.Start();

#if FRUITWARSMODE

            List<int> intlist = new List<int> { 17, 23, 05, 11 };

            Parallel.ForEach(intlist, (i) => service.LeaderboardAtResetStartAsync(new CancellationToken(), i, 02));
#endif

            string Dog = "Images/dog-eyebrow.gif";
            //string Rat = "Images/large-ugly-rat.jpg";
            //string Doge = "Images/dogecoin_head.png";

            connectionManager.AddNewServer("Dog", 901581446618169344, 902080736197242901, Dog, _client);
            //connectionManager.AddExistingServer("Rat", 771103419023753226, 902077849513652234, Rat, _client);
            //connectionManager.AddExistingServer("Woof", 902051653656641557, 902087868258877491, Doge, _client);

            string DarkGrimoire = "Images/DarkGrimoire.png";
            connectionManager.AddNewServer("Dark Grimoire", 625996199597703168, 902989021209317456, DarkGrimoire, _client);


        }

        private void CheckConnection(object sender, ElapsedEventArgs e)
        {
            Environment.ExitCode = 1;
            if (_client.ConnectionState == ConnectionState.Disconnecting)
            {
                Environment.Exit(1);
            }
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.MessageId != 857061904944988160)
            {
                return;
            }

            if (arg3.Emote.Name != "✅")
            {
                return;
            }

            SocketRole role = (arg2 as SocketGuildChannel).Guild.Roles.FirstOrDefault(x => x.Id == 856709182514397194);
            await (arg3.User.Value as SocketGuildUser).AddRoleAsync(role);


        }

        private async Task OnJoinedGuild(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("Bot joined the server");
        }

        private async Task OnChannelCreated(SocketChannel arg)
        {
            if ((arg as ITextChannel) == null)
            {
                return;
            }

            ITextChannel channel = arg as ITextChannel;

            await channel.SendMessageAsync("Channel created");
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message))
            {
                return;
            }

            SocketCommandContext context = new SocketCommandContext(_client, message);
            if (message.Source != MessageSource.User)
            {
                return;
            }

            int argPos = 0;



            if (context.IsPrivate)
            {
                await OnPrivateMessageReceived(arg, context, message, argPos);
                return;
            }


            if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                string nickName = context.Guild.GetUser(context.User.Id).Nickname ?? context.User.Username;

                if (connectionManager.serverAllianceChannelKnown(context.Channel.Id))
                {
                    connectionManager.SendMessageToAllAllianceChannels(context.Channel.Id, nickName, context.Message.Content);
                }


                //REMOVE THIS LATER
                if (context.Message.Content.Equals("clear", StringComparison.OrdinalIgnoreCase))
                {
                    connectionManager.Clear();
                }

            }

            await _service.ExecuteAsync(context, argPos, _provider);


        }

        private async Task OnPrivateMessageReceived(SocketMessage arg, SocketCommandContext context, SocketUserMessage message, int argPos)
        {
            Random rand = new(DateTime.Now.Millisecond);
            List<string> grapeJob = new();
            grapeJob.Add("Also 🍇Grape🍇 is the superior fruit.");
            grapeJob.Add("Also 🍇Grape🍇#1");
            grapeJob.Add("Also if 🍇Grapes🍇 don't win it's rigged.");
            grapeJob.Add("Also 🍇Grapes🍇 control the bot, just sayin.");


            if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }
            else
            {
                string[] buffer = context.Message.ToString().Split(' ');
                string RSNInput = "";

                for (int idx = 1; idx < buffer.Count(); idx++)
                {
                    RSNInput += buffer[idx];
                    if (idx < buffer.Count() - 1 && !buffer[idx].Equals(" "))
                    {
                        RSNInput += " ";
                    }
                }


                string userDiscordTag = context.Message.Author.Username + "#" + context.Message.Author.Discriminator;


            }
            return;
        }//🍇🍌🍎🍑💩

        private async Task OnCommandExecuted(Optional<Discord.Commands.CommandInfo> command, ICommandContext context, IResult result)
        {
            // Error catcher
            if (command.IsSpecified && !result.IsSuccess)
            {
                switch (result.Error)
                {
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.BadArgCount}");
                        break;
                    case CommandError.Exception:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.Exception}");
                        if (result.ErrorReason.Contains("was not present in the dictionary."))
                        {
                            await context.Channel.SendMessageAsync($"It's possible that this could be due to the person specified not being signed up for Fruit Wars.");
                        }

                        break;
                    case CommandError.ObjectNotFound:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.ObjectNotFound}");
                        break;
                    case CommandError.ParseFailed:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.ParseFailed}");
                        break;
                    case CommandError.UnknownCommand:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.UnknownCommand}");
                        break;
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.UnmetPrecondition}");
                        break;
                    case CommandError.Unsuccessful:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.Unsuccessful}");
                        break;
                    case CommandError.MultipleMatches:
                        await context.Channel.SendMessageAsync($"Error type: {CommandError.MultipleMatches}");
                        await context.Channel.SendMessageAsync($"Multiple crash causes detected. This is bad, you should probably look into it.");
                        break;
                }
                await context.Channel.SendMessageAsync($"Error: {result}");
                await context.Channel.SendMessageAsync($"I just caught an error that would have normally crashed me! Now call me a good bot :D");
            }

        }
    }
}