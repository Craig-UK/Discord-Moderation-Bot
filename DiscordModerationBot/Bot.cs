using DiscordModerationBot.commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


/********************************************************************************************
* Class: Bot
* Description: Initialise the bot.
* Developer: Craig Climie
* Last Update: 11/05/2022 at 5.26pm
*******************************************************************************************/

namespace DiscordModerationBot
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InfoCommands Info { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configjson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configjson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Info = new InfoCommands();

            Client.MessageReactionAdded += async (s, e) =>
            {
                DiscordMember member = (DiscordMember)e.User;
                var emoji = DiscordEmoji.FromName(Client, ":white_check_mark:");
                //var role = Info.Role;
                var role = e.Guild.GetRole(858446619132428308);
                if (e.Emoji == emoji)
                {
                    await member.GrantRoleAsync(role);
                }
            };

            Client.MessageReactionRemoved += async (s, e) =>
            {
                DiscordMember member = (DiscordMember)e.User;
                var emoji = DiscordEmoji.FromName(Client, ":white_check_mark:");
                //var role = Info.Role;
                var role = e.Guild.GetRole(858446619132428308);
                if (e.Emoji == emoji)
                {
                    await member.RevokeRoleAsync(role);
                }
            };

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configjson.Prefix }
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<ModerationCommands>();
            Commands.RegisterCommands<InfoCommands>();
            Commands.RegisterCommands<UserCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient dc, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
