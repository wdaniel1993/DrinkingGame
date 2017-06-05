﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using SuccincT.Functional;
using SuccincT.Options;
using SuccincT.Parsers;

namespace DrinkingGame.WebService.Dialogs
{

    [Serializable]
    public class GameDialog : LuisDialog<Object>
    {
        [NonSerialized]
        private IGameService _gameService;
        private Guess _newGuess;

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
                var game = await CurrentGame(context);
                if (game != null)
                {
                    var player = new Player
                    {
                        Name = playerName
                    };
                    if (await TryAction(game.AddPlayer(player), context))
                    {
                        await Answer(context, $"Added player: {playerName}");
                    }
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

            var game = await CurrentGame(context);
            if (game != null)
            {
                if (await TryAction(game.CompleteAddingdPlayers(), context))
                {
                    await Answer(context, $"Completed adding players. Loading new question.", InputHints.IgnoringInput);
                    await Answer(context, $"New Question: {game.CurrentRound.Puzzle.Question}");
                }
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("poldi.intent.game.guess")]
        public async Task AddGuess(IDialogContext context, LuisResult result)
        {
            var guess = result.CompositeEntities.Where(x => x.ParentType == "guess").SelectMany(x => x.Children).ToList();
            var playerName = guess.FirstOrDefault(x => x.Type == "playerName")?.Value;
            var guessAsString = guess.FirstOrDefault(x => x.Type == "builtin.number")?.Value;
            var guessedNumber = string.IsNullOrEmpty(guessAsString) ? Option<int>.None() : guessAsString.TryParseInt();
            if (playerName != null && guessedNumber.HasValue)
            {
                var game = await CurrentGame(context);
                if (game != null)
                {
                    var player = await GetPlayer(game, playerName, context);
                    if (player != null)
                    {
                        _newGuess = new Guess {Player = player, Estimate = guessedNumber.Value};
                        await Answer(context,
                            $"I heared that {_newGuess.Player.Name} guessed {_newGuess.Estimate}.");
                        PromptDialog.Confirm(context, AfterConfirm, "Is this correct?",
                            "Please answer with yes or now. Is this correct?");

                    }
                    else
                    {
                        context.Wait(MessageReceived);
                    }
                }
                else
                {
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await Answer(context, "Could not read guess");
                context.Wait(MessageReceived);
            }
        }

        private async Task AfterConfirm(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                var game = await CurrentGame(context);
                if (game != null)
                {
                    if (await TryAction(game.AddGuess(_newGuess), context))
                    {
                        await Answer(context, "Saved guess.");
                    }
                }
            }
            else
            {
                await Answer(context, "Deleted guess. Try again.");
            }
            _newGuess = null;
            context.Wait(MessageReceived);
        }

        private async Task<Game> CurrentGame(IDialogContext context)
        {
            int gameId;
            context.UserData.TryGetValue<int>("gameId",out gameId);
            
            var game = _gameService.Games.FirstOrDefault(x => x.Id == gameId);

            if (game == null)
            {
                await Answer(context, "Start a game first.");
            }

            return game;
        }

        private async Task<Player> GetPlayer(Game game, string playerName, IDialogContext context)
        {
            var player = game.Players.FirstOrDefault(x => playerName == x.Name || playerName.Contains(x.Name) || x.Name.Contains(playerName));

            if (player == null)
            {
                await Answer(context, "Player not found.");
            }

            return player;
        }

        private async Task<Maybe<T>> TryAction<T>(Task<T> action, IDialogContext context)
        {
            return await action.ToObservable().Select(Maybe<T>.Some)
                .Catch<Maybe<T>, StateException>(exception =>
                {
                    return Answer(context, @"There's a time and place for everything, but not now.")
                        .ToObservable()
                        .Select(_ => Maybe<T>.None());
                });
        }

        private async Task<bool> TryAction(Task action, IDialogContext context)
        {
            return await action.ToObservable().Select(_ => true)
                .Catch<bool,StateException>(exception =>
                {
                    return Answer(context, @"There's a time and place for everything, but not now.")
                        .ToObservable()
                        .Select(_ => false);
                });
        }

        private async Task Answer(IDialogContext context, string message, string inputHint = InputHints.ExpectingInput)
        {
            await context.SayAsync(message,message,new MessageOptions
            {
                InputHint = inputHint
            });
        }
    }
}