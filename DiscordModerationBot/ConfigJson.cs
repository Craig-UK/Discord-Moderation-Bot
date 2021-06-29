using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


/********************************************************************************************
* Class: ConfigJson
* Description: Gets the token and prefix of the bot.
* Developer: ScyferHQ
* Last Update: 10/05/2021 at 1:52am
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
