﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


/********************************************************************************************
* Class: InfoCommands
* Description: Contains commands that gets information about the server, bot, user or roles.
* Developer: ScyferHQ
* Last Update: 23/05/2021 at 11:27pm
*******************************************************************************************/

namespace DiscordModerationBot.commands
{
    class InfoCommands : BaseCommandModule
    {
        [Command("status")]
        [Description("Displays the name, status, author and profile picture of the bot.")]
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
        [Description("Displays the user's name, discriminator(#0000), profile picture, when the user created their account and when the user joined the Server/Guild. " +
                     "Defaults to the user who initiated the command.")]
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

        [Command("roleinfo")]
        [Description("Displays the role's name, colour, when the role was created and the permissions that the role has. ")]
        public async Task RoleInfoAsync(CommandContext ctx, 
                                        [RemainingText][Description("Role to get information about.")] DiscordRole role)
        {
            if(role == null)
            {
                var roleInfoErrorEmbed = new DiscordEmbedBuilder
                {
                    Title = "An Error Occurred.",
                    Description = "Please @ mention a role to see its information.",
                    Color = DiscordColor.Red,
                    Timestamp = DateTime.Now
                };

                await ctx.Channel.SendMessageAsync(embed: roleInfoErrorEmbed).ConfigureAwait(false);
            }
            else
            {
                var roleInfoEmbed = new DiscordEmbedBuilder
                {
                    Title = role.Name + "'s Info",
                    Description = "Role name: " + role.Name + 
                                  "\nRole Colour: " + role.Color +
                                  "\nCreated On: " + role.CreationTimestamp +
                                  "\nPermissions: " + role.Permissions.ToPermissionString(),
                    Color = role.Color,
                    Timestamp = DateTime.Now
                };

                await ctx.Channel.SendMessageAsync(embed: roleInfoEmbed).ConfigureAwait(false);
            }
        }

        [Command("guildinfo")]
        [Aliases("serverinfo")]
        [Description("Displays the name of the server, amount of members and when the server was created.")]
        public async Task ServerInfoAsync(CommandContext ctx)
        {
            var serverInfoEmbed = new DiscordEmbedBuilder
            {
                Title = $"Information about {ctx.Guild.Name}",
                Description = $"Name of the server/guild: {ctx.Guild.Name}" +
                              $"\nMembers in the server: {ctx.Guild.MemberCount}" +
                              $"\nCreated On: {ctx.Guild.CreationTimestamp}",
                ImageUrl = ctx.Guild.BannerUrl,
                Color = ctx.Member.Color,
                Timestamp = DateTime.Now
            };

            var serverInfoMessage = await ctx.Channel.SendMessageAsync(embed: serverInfoEmbed).ConfigureAwait(false);
        }
    }
}
