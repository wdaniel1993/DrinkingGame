using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace DrinkingGame.WebService.Dialogs
{

    [Serializable]
    public class GameDialog : LuisDialog<Object>
    {
        [NonSerialized]
        private IGameService _gameService;


        public GameDialog(ILuisService luisService): base(luisService)
        {
            ResolveDependencies();   
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            _gameService = DependencyManager.Current.Resolve<IGameService>();
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: " + string.Join(", ", result.Query);
            await Answer(context,message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("poldi.intent.game.start")]
        public async Task StartGame(IDialogContext context, LuisResult result)
        {
            var gameId = _gameService.StartNewGame();
            context.UserData.SetValue("gameId", gameId);
            string message = $"Started new game with ID {gameId}. Please add users.";
            await Answer(context,message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("poldi.intent.game.addplayer")]
        public async Task AddPlayer(IDialogContext context, LuisResult result)
        {
            var playerName = result.Entities.FirstOrDefault(x => x.Type == "playerName")?.Entity;
            if (!string.IsNullOrEmpty(playerName))
            {
                string message = $"Added player: {playerName}";

                var game = CurrentGame(context);
                if (game != null)
                {
                    await game
                        .AddPlayer(new Player
                        {
                            Name = playerName
                        });

                    await Answer(context,message);
                }
                else
                {
                    await Answer(context,"Start a game first.");
                }
            }
            else
            {
                await Answer(context,"Could not get name.");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("poldi.intent.game.finishplayers")]
        public async Task FinishedAddingPlayers(IDialogContext context, LuisResult result)
        {

            var game = CurrentGame(context);
            if (game != null)
            {
                await game.CompleteAddingdPlayers();
                await Answer(context,$"Completed adding players.");
            }
            else
            {
                await Answer(context,"Start a game first.");
            }
            context.Wait(MessageReceived);
        }

        private Game CurrentGame(IDialogContext context)
        {
            int gameId;
            context.UserData.TryGetValue<int>("gameId",out gameId);
            return _gameService.Games.FirstOrDefault(x => x.Id == gameId);
        }

        private async Task Answer(IDialogContext context, string message)
        {
            await context.SayAsync(message,message,new MessageOptions
            {
                InputHint = InputHints.ExpectingInput
            });
        }
    }
}