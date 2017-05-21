using DrinkingGame.Client.Signalr;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.Shared.DataTransfer;

namespace DrinkingGame.Client.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunGame().Wait();
        }

        private static async Task RunGame()
        {
            var hubConnection = new HubConnection(@"http://localhost:62562/");
            var hubProxy = new DrinkingGameHubProxy(hubConnection, "DrinkingGameHub");

            hubConnection.TraceLevel = TraceLevels.All;
            hubConnection.TraceWriter = System.Console.Out;

            await hubConnection.Start();
            

            hubProxy.CorrectAnswer.Subscribe(System.Console.WriteLine);
            hubProxy.NewAnswer.Subscribe(System.Console.WriteLine);
            hubProxy.NewRound.Subscribe(System.Console.WriteLine);
            hubProxy.ShouldDrink.Subscribe(System.Console.WriteLine);
            hubProxy.UpdateGameDetails.Subscribe(System.Console.WriteLine);
            hubProxy.UpdateScores.Subscribe(System.Console.WriteLine);

            int? gameNumber;
            do
            {
                System.Console.WriteLine("Spielnummer:");
                gameNumber = ReadNumber();
            } while (!gameNumber.HasValue);
            await hubProxy.ConnectToGame(new ConnectToGameDto
            {
                GameNumber = gameNumber.Value,
                SupportShouldDrink = true
            });

            while (true)
            {
                System.Console.WriteLine("Spieler:");
                var player = System.Console.ReadLine();
                System.Console.WriteLine("Aktion (1 = Raten, 2 = Trinken):");
                var action = ReadNumber();
                if (action.HasValue)
                {
                    switch (action)
                    {
                        case 1:
                            System.Console.WriteLine("Raten:");
                            var guess = ReadNumber();
                            if (guess.HasValue)
                            {
                                await hubProxy.GaveAnswer(new GaveAnswerDto
                                {
                                    Player = player,
                                    Answer = guess.Value
                                });
                            }
                            break;
                        case 2:
                            await hubProxy.PlayerDrank(new PlayerDrankDto
                            {
                                Player = player
                            });
                            break;
                        default:
                            System.Console.WriteLine("Ungültige Aktion");
                            break;
                    }
                }
            }
        }

        private static int? ReadNumber()
        {
            var inputString = System.Console.ReadLine();
            if (int.TryParse(inputString,out var readNumber))
            {
                return readNumber;
            }
            return null;
        }
    }
}
