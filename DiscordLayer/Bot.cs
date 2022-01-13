using DiscordLayer.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLayer
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        private const string databaseName = "FImonDB";

        public async Task RunAsync()
        {
            string jsonBotConfigString = "";

            using (FileStream fs = File.OpenRead("config.json"))
            using (StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
                jsonBotConfigString = await sr.ReadToEndAsync().ConfigureAwait(false);

            ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(jsonBotConfigString);

            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            InteractivityConfiguration interactivityConfig = new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            };

            Client.UseInteractivity(interactivityConfig);

            CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                CaseSensitive = false,
                DmHelp = true,
                IgnoreExtraArguments = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<BaseCommands>();
            Commands.RegisterCommands<StudentCommands>();
            Commands.RegisterCommands<TeacherCommands>();
            Commands.RegisterCommands<AdminCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
