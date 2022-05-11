using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


/********************************************************************************************
* Class: FunCommands
* Description: Contains commands that are random and fun.
* Developer: Craig Climie
* Last Update: 11/05/2021 at 5.28pm
*******************************************************************************************/

namespace DiscordModerationBot
{
    class FunCommands : BaseCommandModule 
    {
        [Command("ping")]
        [Description("Returns pong.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds 2 numbers together.")]
        [RequireRoles(RoleCheckMode.Any, "Admin", "Member")]
        public async Task Add(CommandContext ctx, 
            [Description("First number")] int num1, 
            [Description("Second number.")] int num2)
        {
            await ctx.Channel.SendMessageAsync((num1 + num2).ToString());
        }

        [Command("hello")]
        public async Task Hello(CommandContext ctx)
        {
            var helloEmbed = new DiscordEmbedBuilder
            {
                Title = "Hello, " + ctx.User.Username + ".",
                ImageUrl = ctx.User.AvatarUrl,
                Color = DiscordColor.Green
            };

            var helloMessage = await ctx.Channel.SendMessageAsync(embed: helloEmbed).ConfigureAwait(false);
        }

        [Command("test")]
        public async Task Join(CommandContext ctx)
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Title = "Test embed.",
                ImageUrl = ctx.User.AvatarUrl,
                Color = DiscordColor.Green
            };

            var testMessage = await ctx.Channel.SendMessageAsync(embed: testEmbed).ConfigureAwait(false);

            var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await testMessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
            await testMessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == testMessage &&
                x.User == ctx.User &&
                (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == thumbsUpEmoji)
            {
                var role = ctx.Guild.GetRole(841444470516088883);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
        }
    }
}
