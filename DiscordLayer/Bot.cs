using DiscordLayer.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace DiscordLayer
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            //This should be injected
            Config config = new Config
            {
                Token = Environment.GetEnvironmentVariable("PV178StudyBot_Token"),
                Prefix = Environment.GetEnvironmentVariable("PV178StudyBot_Prefix")
            };

            DiscordConfiguration discordConfig = new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += OnClientReady;

            InteractivityConfiguration interactivityConfig = new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            };

            Client.UseInteractivity(interactivityConfig);

            CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { config.Prefix },
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
