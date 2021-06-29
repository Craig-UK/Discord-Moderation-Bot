using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;


/********************************************************************************************
* Class: ModerationCommands
* Description: Contains commands that will be used for moderation purposes.
* Developer: ScyferHQ
* Last Update: 27/06/2021 at 4.36pm
*******************************************************************************************/

namespace DiscordModerationBot.commands
{
    class ModerationCommands : BaseCommandModule
    {
        public DiscordChannel LogsChannel { get; private set; }

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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

            await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

                    await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

                    await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

            if (reason == null)
            {
                var muteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Muted",
                    Description = member.Username + "#" + member.Discriminator + " was muted from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + numSeconds + " seconds ",
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = muteMemberEmbed
                };

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
            }
            else
            {
                var muteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Muted",
                    Description = member.Username + "#" + member.Discriminator + " was muted from " + ctx.Guild.Name + " at " + ctx.Message.Timestamp.DateTime + " for " + reason + 
                                  " for " + numSeconds + " seconds ",
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = muteMemberEmbed
                };

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
            }

            //System.Timers.Timer timer;
            TimeSpan time = TimeSpan.FromSeconds(numSeconds);
            await Task.Delay(time);
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

            DiscordChannel channel = await GetChannelNamesAsync(ctx, LogsChannel.Name);

            if (reason == null)
            {
                var unmuteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unmuted",
                    Description = member.Username + "#" + member.Discriminator + " was unmuted from " + ctx.Guild.Name + " at " + DateTime.Now,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unmuteMemberEmbed
                };

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
            }
            else
            {
                var unmuteMemberEmbed = new DiscordEmbedBuilder
                {
                    Title = "Member Unmuted",
                    Description = member.Username + "#" + member.Discriminator + " was unmuted from " + ctx.Guild.Name + " at " + DateTime.Now + " for " + reason,
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = unmuteMemberEmbed
                };

                await messageToSend.SendAsync(channel).ConfigureAwait(false);
            }
        }

        [Command("setlogschannel")]
        [Description("Set the channel that all logs will be sent to.")]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator")]
        public async Task SetLogsChannelAsync(CommandContext ctx, 
                                              [Description("Channel name (#[channelname]) or channel id (18 number id) to send your server's logs to.")] DiscordChannel logs)
        {
            LogsChannel = await GetChannelNamesAsync(ctx, logs.Name);
            var logsChannelSet = new DiscordEmbedBuilder
            {
                Title = "Logs Channel Successfully set!",
                Description = $"Logs channel was successfully set to {LogsChannel.Name} for Server {ctx.Guild.Name}",
                Timestamp = DateTime.Now,
                Color = DiscordColor.Green,
                ImageUrl = ctx.Guild.BannerUrl
            };

            await ctx.Channel.SendMessageAsync(embed: logsChannelSet).ConfigureAwait(false);
        }

        //[Command("getchannels")]
        //public async Task<DiscordChannel> GetChannelNamesAsync(CommandContext ctx, string channelName)
        //{
        //    var channels = await ctx.Guild.GetChannelsAsync();
        //    var selectedChannels = channels.Where(c => c.Name == channelName);
        //    foreach (DiscordChannel discordChannel in selectedChannels)
        //    {
        //        return discordChannel;
        //    }
        //    return null;
        //}

        public async Task<DiscordChannel> GetChannelNamesAsync(CommandContext ctx, string channelName)
        {
            var channels = await ctx.Guild.GetChannelsAsync();
            var selectedChannels = channels.Where(c => c.Name == channelName);
            foreach (DiscordChannel discordChannel in selectedChannels)
            {
                return discordChannel;
            }
            return null;
        }
    }
}
