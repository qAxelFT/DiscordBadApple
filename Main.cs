using System;
using System.Threading.Tasks;
using Discord;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Token;

public class main
{
    private DiscordSocketClient _bot;
    private CommandService _commands;
    private IServiceProvider _services;
    
    public static void Main(string[] args)
        => new main().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {   
        _bot = new DiscordSocketClient();
        _commands = new CommandService();
        
        _services = new ServiceCollection()
            .AddSingleton(_bot)
            .AddSingleton(_commands)
            .BuildServiceProvider();     

        _bot.Log += _bot_Log;

        await RegisterCommandsAsync();

        await _bot.LoginAsync(TokenType.Bot, Token.botToken.token);

        await _bot.StartAsync();

        await Task.Delay(-1);
    }

    private Task _bot_Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
    public async Task RegisterCommandsAsync()
    {
        _bot.MessageReceived += HandleCommandAsync;
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage msg)
    {
        var message = msg as SocketUserMessage;
        var context = new SocketCommandContext(_bot, message);
        if (message.Author.IsBot)
            return;

        int argPos = 0;

        if(message.HasStringPrefix("!", ref argPos))
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if(!result.IsSuccess)
                Console.WriteLine(result.ErrorReason);
        }
    }
}


/*
string text = File.ReadAllText(@"./src/play.txt");

string rawFrame = text.Replace(",", " ");
        
string[] frames = rawFrame.Split("SPLIT");

foreach(var frame in frames)
    {
        Console.Clear();
        Console.WriteLine(frame);
        Thread.Sleep(100);
    }
*/
