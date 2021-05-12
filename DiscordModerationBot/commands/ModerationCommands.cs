using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordModerationBot.commands
{
    class ModerationCommands : BaseCommandModule
    {
        [Command("purge")]
        [Description("Delete the specified amount of messages from the current channel.")]
        [RequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task DeleteMessagesAsync(CommandContext ctx, 
                                              [Description("Number of messages to delete.")] int messages)
        {
            IEnumerable<DiscordMessage> numMessages = await ctx.Channel.GetMessagesAsync(messages);
            await ctx.Channel.DeleteMessagesAsync(numMessages);

            var bulkDeleteEmbed = new DiscordEmbedBuilder
            {
                Title = "Messages Deleted",
                Description = ctx.Member.Username + " Deleted " + messages + " messages in channel " + ctx.Channel.Name + " at " + ctx.Message.Timestamp.DateTime,
                Timestamp = DateTime.Now,
                ImageUrl = ctx.User.AvatarUrl
            };
            
            // This section sends a message into the "logs"
            // channel.
            var messageToSend = new DiscordMessageBuilder
            {
                Embed = bulkDeleteEmbed
            };

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            await messageToSend.SendAsync(logs).ConfigureAwait(false);
        }

        [Command("ban")]
        [Description("Bans the specified member from the Guild(Server).")]
        [RequirePermissions(DSharpPlus.Permissions.BanMembers)]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task BanMemberAsync(CommandContext ctx, 
                                         [Description("Member to ban.")] DiscordMember member, 
                                         [Description("The amount of days to delete messages from this member.")] int delete_message_days = 0, 
                                         [Description("Reason for banning this member.")] string reason = null)
        {
            await member.BanAsync(delete_message_days, reason);
            await ctx.Guild.BanMemberAsync(member, delete_message_days, reason);

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            if (delete_message_days == 0 && reason == null)
            {
                var banMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Banned",
                    Description = member.Username + "#" + member.Discriminator + " was banned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = banMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }

            if (delete_message_days > 0 && reason == null)
            {
                var banMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Banned",
                    Description = member.Username + "#" + member.Discriminator + " was banned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = banMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
            else
            {
                if (delete_message_days > 0 && reason != null)
                {
                    var banMemberEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Member Banned",
                        Description = member.Username + "#" + member.Discriminator + " was banned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason,
                        Timestamp = DateTime.Now,
                        ImageUrl = ctx.User.AvatarUrl
                    };

                    // This section sends a message into the "logs"
                    // channel.
                    var messageToSend = new DiscordMessageBuilder
                    {
                        Embed = banMemberEmbed
                    };

                    await messageToSend.SendAsync(logs).ConfigureAwait(false);
                }
                if (delete_message_days == 0 && reason != null)
                {
                    var banMemberEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Member Banned",
                        Description = member.Username + "#" + member.Discriminator + " was banned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason,
                        Timestamp = DateTime.Now,
                        ImageUrl = ctx.User.AvatarUrl
                    };

                    // This section sends a message into the "logs"
                    // channel.
                    var messageToSend = new DiscordMessageBuilder
                    {
                        Embed = banMemberEmbed
                    };

                    await messageToSend.SendAsync(logs).ConfigureAwait(false);
                }
            }
        }

        [Command("unban")]
        [Description("Unbans the specified member from the Guild(Server).")]
        [RequirePermissions(DSharpPlus.Permissions.BanMembers)]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task UnbanMemberAsync(CommandContext ctx, DiscordUser user, string reason = null)
        {
            await user.UnbanAsync(ctx.Guild, reason).ConfigureAwait(false);

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            if (reason == null)
            {
                var unbanMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unbanned",
                    Description = user.Username + "#" + user.Discriminator + " was unbanned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unbanMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
            else
            {
                var unbanMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unbanned",
                    Description = user.Username + "#" + user.Discriminator + " was banned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unbanMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
        }
    }
}
