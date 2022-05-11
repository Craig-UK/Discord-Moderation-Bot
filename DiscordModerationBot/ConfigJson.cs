using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


/********************************************************************************************
* Class: ConfigJson
* Description: Gets the token and prefix of the bot.
* Developer: Craig Climie
* Last Update: 11/05/2022 at 5.26pm
*******************************************************************************************/

namespace DiscordModerationBot
{
    class ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}
