using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordModerationBot.commands
{
    class InfoCommands : BaseCommandModule
    {
        [Command("status")]
        [Description("Displays the name, status, author and profile picture of the bot.")]
        [RequireRoles(RoleCheckMode.All)]
        public async Task BotStatusAsync(CommandContext ctx)
        {
            var statusEmbed = new DiscordEmbedBuilder
            {
                Title = "Bot Status",
                Description = ctx.Client.CurrentUser.Username + "#" + ctx.Client.CurrentUser.Discriminator +
                              "\nBot Status: " + UserStatus.Online +
                              "\nBot Author: ScyferHQ",
                Color = DiscordColor.Green,
                ImageUrl = ctx.Member.AvatarUrl,
                Timestamp = DateTime.Now
            };

            var statusMessage = await ctx.Channel.SendMessageAsync(embed: statusEmbed).ConfigureAwait(false);
        }

        [Command("userinfo")]
        [Description("Displays the name, status, author and profile picture of the user mentioned. Defaults to the user who initiated the command.")]
        [RequireRoles(RoleCheckMode.All)]
        public async Task UserInfoAsync(CommandContext ctx, 
                                        [Description("User to check info for. Defaults to the user who initiated the command.")] DiscordUser user = null)
        {
            if(user == null)
            {
                var currentUserEmbed = new DiscordEmbedBuilder
                {
                    Title = ctx.User.Username + "'s Info",
                    Description = ctx.User.Username + "#" + ctx.User.Discriminator +
                              "\nCreated On: " + ctx.User.CreationTimestamp +
                              "\nJoined " + ctx.Guild.Name + " On: " + ctx.Member.JoinedAt,
                    ImageUrl = ctx.User.AvatarUrl,
                    Timestamp = DateTime.Now
                };

                await ctx.Channel.SendMessageAsync(embed: currentUserEmbed).ConfigureAwait(false);
            } 
            else
            {
                DiscordMember member = (DiscordMember) ctx.User;
                var currentUserEmbed = new DiscordEmbedBuilder
                {
                    Title = user.Username + "'s Info",
                    Description = user.Username + "#" + user.Discriminator +
                              "\nCreated On: " + user.CreationTimestamp +
                              "\nJoined " + ctx.Guild.Name + " On: " + member.JoinedAt,
                    ImageUrl = user.AvatarUrl,
                    Timestamp = DateTime.Now
                };

                await ctx.Channel.SendMessageAsync(embed: currentUserEmbed).ConfigureAwait(false);
            }
        }

        [Command("credits")]
        [Description("Displays a list of everyone who helped develop and test the bot.")]
        [RequireRoles(RoleCheckMode.All)]
        public async Task CreditsAsync(CommandContext ctx) 
        {
            var creditsEmbed = new DiscordEmbedBuilder
            {
                Title = "Special Thanks to everyone on this list!",
                Description = "Developers:\n" +
                              "ScyferHQ\n" +
                              "Testers:\n" +
                              "Dayne, aka Orion/UltimateOrion\n" +
                              "C41cken\n" +
                              "Laserblaster13\n" +
                              "Bernardo, aka 4raer",
                Color = DiscordColor.Cyan,
                Timestamp = DateTime.Now
            };

            await ctx.Channel.SendMessageAsync(embed: creditsEmbed).ConfigureAwait(false);
        }
    }
}
