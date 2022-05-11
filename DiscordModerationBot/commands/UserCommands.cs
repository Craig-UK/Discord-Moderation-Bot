using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


/********************************************************************************************
* Class: UserCommands
* Description: Contains commands that does something with a user.
* Developer: Craig Climie
* Last Update: 11/05/2022 at 5.27pm
*******************************************************************************************/

namespace DiscordModerationBot.commands
{
    class UserCommands : BaseCommandModule 
    {
        [Command("nick")]
        [Aliases("nickname")]
        [Description("Change the nickname of the specified user.")]
        [RequirePermissions(DSharpPlus.Permissions.ManageNicknames)]
        [RequireRoles(RoleCheckMode.Any, "Owner", "Admin", "Administrator", "Mod", "Moderator", "BOT")]
        public async Task ChangeNicknameAsync(CommandContext ctx, 
                                              [Description("Member to change the nickname for.")] DiscordMember member, 
                                              [RemainingText][Description("The nickname to give to the user.")] string new_nickname)
        {
            try
            {
                await member.ModifyAsync(m =>
                {
                    m.Nickname = new_nickname;
                    m.AuditLogReason = $"Changed by {ctx.User.Username}.";
                });

                var success = new DiscordEmbedBuilder
                {
                    Title = $"{member.Username}'s nickname was successully changed.",
                    Description = $"{member.Username}'s nickname was successully changed to {new_nickname} at {ctx.Message.Timestamp}.",
                    Color = DiscordColor.Green,
                    Timestamp = DateTime.Now
                };

                var successMessage = await ctx.Channel.SendMessageAsync(embed: success).ConfigureAwait(false);

                var memberNicknameChanged = new DiscordEmbedBuilder
                {
                    Title = $"{member.Username}'s nickname was changed",
                    Description = $"{member.Username}'s nickname was changed by {ctx.User.Username} at {ctx.Message.Timestamp}.",
                    Timestamp = DateTime.Now,
                    ImageUrl = ctx.User.AvatarUrl
                };

                // This section sends a message into the "logs"
                // channel.
                var messageToSend = new DiscordMessageBuilder
                {
                    Embed = memberNicknameChanged
                };

                DiscordChannel logs = await ctx.Client.GetChannelAsync(841716264454848555);

                await messageToSend.SendAsync(logs).ConfigureAwait(false);
            } 
            catch (Exception ex)
            {
                var error = new DiscordEmbedBuilder
                {
                    Title = "An error occurred.",
                    Description = $"An error occurred while trying to change {member.Username}'s nickname." +
                                  $"\nError: {ex.Message}.",
                    Color = DiscordColor.Red,
                    Timestamp = DateTime.Now
                };

                await ctx.Channel.SendMessageAsync(embed: error).ConfigureAwait(false);
            }
        }
    }
}
