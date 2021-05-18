using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Play;

public class Commands : ModuleBase<SocketCommandContext>
    {
    public static bool isRunning = false;
    public static List<DateTimeOffset> stackCooldownTimer = new List<DateTimeOffset>();
    public static List<SocketGuildUser> stackCooldownTarget = new List<SocketGuildUser>();

    [Command("BadApple")]
    public async Task Bad()
    {
        ThreadStart ts = new ThreadStart(BadApple.play);
        Thread backgroundThread = new Thread(ts);
        string usrname = Environment.UserName;
            
        if(Commands.stackCooldownTarget.Contains(Context.User as SocketGuildUser))
        {
            if(Commands.stackCooldownTimer[Commands.stackCooldownTarget.IndexOf(Context.Message.Author as SocketGuildUser)].AddMinutes(8) >= DateTimeOffset.Now)
            {
                int minutesLeft = (int) (Commands.stackCooldownTimer[Commands.stackCooldownTarget.IndexOf(Context.Message.Author as SocketGuildUser)].AddMinutes(8) - DateTimeOffset.Now).TotalMinutes;
                await ReplyAsync($"Hey! {Context.Message.Author}, you're currently on cooldown, please wait {minutesLeft} minutes");
                return;
            }
            else
            {
                Commands.stackCooldownTimer[Commands.stackCooldownTarget.IndexOf(Context.Message.Author as SocketGuildUser)] = DateTimeOffset.Now;
            }
        }
        else
        {
            Commands.stackCooldownTarget.Add(Context.User as SocketGuildUser);
            Commands.stackCooldownTimer.Add(DateTimeOffset.Now);
        }
        if(isRunning)
        {
            await ReplyAsync("Wait! Bad Apple is currently playing, please wait.");
        }
        else
        {       
            var msg = await Context.Channel.SendMessageAsync($"Starting Bad Apple on {usrname}'s computer");
            
            backgroundThread.Start();
        }
    }

    [Command("TimeLeft")]
    public async Task TimeLeft()
    {
        if(isRunning)
        {
            await ReplyAsync($"There are {Play.BadApple.getTimeElapsed()} seconds left");
        }
        else
        {
            await ReplyAsync("Bad Apple is not playing currently, use !BadApple");
        }
    }
}    
