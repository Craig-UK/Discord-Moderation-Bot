using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


/********************************************************************************************
* Class: ModerationCommands
* Description: Contains commands that will be used for moderatiion purposes.
* Developer: ScyferHQ
* Last Update: 23/05/2021 at 9.39pm
*******************************************************************************************/

namespace DiscordModerationBot.commands
{
    class ModerationCommands : BaseCommandModule
    {
        [Command("purge")]
        [Description("Delete the specified amount of messages from the current channel.")]
        [RequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
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
                                         [Description("Reason for banning this member.")][RemainingText]string reason = null)
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
        public async Task UnbanMemberAsync(CommandContext ctx, 
                                           [Description("User to unban.")] DiscordUser user,
                                           [Description("Reason for unbanning this user.")][RemainingText]string reason = null)
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
                    Description = user.Username + "#" + user.Discriminator + " was unbanned from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason,
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

        [Command("kick")]
        [Description("Kicks the specified member from the Guild(Server).")]
        [RequirePermissions(DSharpPlus.Permissions.KickMembers)]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task KickMemberAsync(CommandContext ctx, 
                                          [Description("Member to kick.")]DiscordMember member,
                                          [Description("Reason for kicking this member.")][RemainingText]string reason = null)
        {
            await member.RemoveAsync(reason);

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            if (reason == null)
            {
                var kickMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Kicked",
                    Description = member.Username + "#" + member.Discriminator + " was kicked from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = kickMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
            else
            {
                var kickMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Kicked",
                    Description = member.Username + "#" + member.Discriminator + " was kicked from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = kickMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
        }

        [Command("mute")]
        [Description("Mutes the specified member in the Guild(Server).")]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task TextMuteMemberAsync(CommandContext ctx, 
                                              [Description("Member to mute.")]DiscordMember member, 
                                              [Description("Amount of time to mute this member for (in seconds).")]int numSeconds,
                                              [Description("Reason for muting this member.")][RemainingText]string reason)
        {
            var role = ctx.Guild.GetRole(842550847904677938);
            await member.GrantRoleAsync(role).ConfigureAwait(false);

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            if (reason == null)
            {
                var muteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Muted",
                    Description = member.Username + "#" + member.Discriminator + " was muted from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + numSeconds*60 + " seconds ",
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = muteMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
            else
            {
                var muteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Muted",
                    Description = member.Username + "#" + member.Discriminator + " was muted from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason + 
                                  " for " + numSeconds*60 + " seconds ",
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = muteMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }


            numSeconds = numSeconds * 60;
            System.Timers.Timer timer;

            while (numSeconds > 0)
            {
                timer = new System.Timers.Timer(numSeconds * 1000);
                await Task.Delay(1000);
                numSeconds--;
            }
            await TextUnmuteMemberAsync(ctx, member, "Automatically unmuted by " + ctx.Client.CurrentUser.Username);

            //if (numSeconds == 0)
            //{
            //    await TextUnmuteMemberAsync(ctx, member, "Automatically unmuted by " + ctx.Client.CurrentUser.Username);
            //}
        }

        [Command("unmute")]
        [Description("Unmutes the specified member in the Guild(Server).")]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task TextUnmuteMemberAsync(CommandContext ctx, 
                                                [Description("Member to unmute.")]DiscordMember member,
                                                [Description("Reason for unmuting this member.")][RemainingText]string reason = null)
        {
            var role = ctx.Guild.GetRole(842550847904677938);
            await member.RevokeRoleAsync(role, reason);

            DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

            string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

            if (reason == null)
            {
                var unmuteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unmuted",
                    Description = member.Username + "#" + member.Discriminator + " was unmuted from " + ctx.Guild.Name + " at " + date,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unmuteMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
            else
            {
                var unmuteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unmuted",
                    Description = member.Username + "#" + member.Discriminator + " was unmuted from " + ctx.Guild.Name + " at " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":"
                                  + DateTime.Now.Second + " for " + reason,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unmuteMemberEmbed
                };

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            }
        }
    }
}
