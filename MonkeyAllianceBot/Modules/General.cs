using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyAllianceBot.Modules
{
    public class General : ModuleBase
    {
        private readonly ILogger<General> _logger;

        public General(ILogger<General> logger)
        {
            _logger = logger;
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Alias("Restart", "Update")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        public async Task Stop()
        {
            _logger.LogInformation($"{Context.User.Username} executed the stop command! Exiting now.");
            await Context.Channel.SendMessageAsync("Stop command acknowledged. Restarting now. If I'm not back within 30 seconds, cry for help.", messageReference: new(Context.Message.Id));
            Environment.Exit(0);
        }


    }
}
