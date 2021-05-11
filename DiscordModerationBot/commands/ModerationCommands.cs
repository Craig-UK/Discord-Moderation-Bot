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
                Description = ctx.Member.Username + " Deleted " + messages + " messages in channel " + ctx.Channel.Name + " at " + ctx.Message.Timestamp,
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

            await messageToSend.SendAsync(logs);
        }
    }
}
