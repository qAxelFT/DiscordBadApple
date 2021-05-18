using System;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Play
{
    public class BadApple
    {
        public static int videoTime = 218;
        public static Stopwatch sw = new Stopwatch();

        public static int te = 0; 
        
        public static void play()
        {
            string text = File.ReadAllText(@"./src/play.txt");
            string rawFrame = text.Replace(",", " ");
            string[] frames = rawFrame.Split("SPLIT");

            Commands.isRunning = true;

            sw.Start();

            foreach(string frame in frames)
            {             
                te = (int) sw.Elapsed.TotalSeconds;
                Console.Clear();
                Console.WriteLine(frame);
                Thread.Sleep(100);
            }
            
            Console.Clear();
            sw.Stop();
            sw.Reset();
            Commands.isRunning = false;
        }
        public static int getTimeElapsed()
        {
            int deltaTime = videoTime - te;
            return deltaTime;
        }
    }
}