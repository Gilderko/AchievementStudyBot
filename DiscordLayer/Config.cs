using Newtonsoft.Json;

namespace DiscordLayer
{
    public struct Config
    {
        [JsonProperty("token")]
        public string Token { get; init; }

        [JsonProperty("prefix")]
        public string Prefix { get; init; }
    }
}
