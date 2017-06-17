using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Dialogs;
using NLog;
using SuccincT.Options;
using SuccincT.Parsers;
using SuccincT.PatternMatchers.GeneralMatcher;
using SuccincT.Unions;

namespace DrinkingGame.WebService.Speechlets
{
    public class GameSpeechlet : SpeechletAsync
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private IGameService _gameService;

        private const string GAMEID_KEY = "gameId";
        private const string PLAYERNAME_SLOT = "PlayerName";
        private const string GUESSEDNUMBER_SLOT = "GuessedNumber";

        public GameSpeechlet()
        {
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            _gameService = DependencyManager.Current.Resolve<IGameService>();
        }

        public override bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc,
            SpeechletRequestEnvelope requestEnvelope)
        {
            Log.Info("OnRequestValidation requestId={0}, result={1}", requestEnvelope.Request.RequestId, result);
            return result == SpeechletRequestValidationResult.OK;
        }

        public override async Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        {
            Log.Info("OnIntent requestId={0}, sessionId={1}", intentRequest.RequestId, session.SessionId);
            if (intentRequest.Intent != null)
            {
                var intent = intentRequest.Intent;
                if ("StartGame".Equals(intent.Name))
                {
                    return await StartGame(intent, session);
                }
                else
                {
                    var game = await CurrentGame(session);
                    if (game.HasValue)
                    {
                        switch (intent.Name)
                        {
                            case "AddPlayer": return await AddPlayer(intent, game.Value);
                            case "FinishPlayers": return await FinishPlayers(intent, game.Value);
                            case "Guess": return await NewGuess(intent, game.Value);
                            case "NextRound": return await NextRound(intent, game.Value);
                            case "EndGame": return await EndGame(intent, game.Value);
                        }
                    }
                    else
                    {
                        return BuildSpeechletResponse("Kein Spiel",
                            "Du musst zuerst ein Spiel starten. Schon jetzt betrunken?", false);
                    }
                }
            }
            return BuildSpeechletResponse("Unklare Frage",
                "Ich habe dich leider nicht verstanden", false);
        }

        private async Task<SpeechletResponse> EndGame(Intent intent, Game game)
        {
            var message = new StringBuilder();
            if (!game.CurrentRound.DrinksCompleted)
            {
                message.AppendLine("Ein paar Schwachstellen haben noch nicht getrunken. Prost");
                await game.IgnoreDrinks();
            }
            var actionRes = await TryAction(game.CompleteCurrentRound(true));
            if (actionRes.IsFailure)
            {
                return BuildSpeechletResponse("Fehler", "Ich konnte das Spiel nicht beenden", false);
            }
            else
            {
                message.AppendLine(WriteScores(game));
                message.AppendLine("Spiel beendet");
                return BuildSpeechletResponse("Spiel beendet", message.ToString(), true);
            }
        }

        private async Task<SpeechletResponse> NextRound(Intent intent, Game game)
        {
            var message = new StringBuilder();
            if (!game.CurrentRound.DrinksCompleted)
            {
                message.AppendLine("Ein paar Schwachstellen haben noch nicht getrunken. Prost");
                await game.IgnoreDrinks();
            }
            var actionRes = await TryAction(game.CompleteCurrentRound(false));
            if (actionRes.IsFailure)
            {
                return BuildSpeechletResponse("Fehler", "Ich konnte das Spiel nicht beenden", false);
            }
            else
            {
                message.AppendLine(WriteScores(game));
                message.AppendLine($"Neue Frage: {game.CurrentRound.Puzzle.Question}");
                return BuildSpeechletResponse("Nächste Runde", message.ToString(), false);
            }
        }

        private async Task<SpeechletResponse> NewGuess(Intent intent, Game game)
        {
            var playerName = intent.Slots.TryGetValue(PLAYERNAME_SLOT).Map(x => x.Value);
            var guessedNumber = intent.Slots.TryGetValue(GUESSEDNUMBER_SLOT).Map(x => x.Value.TryParseInt()).Flatten();
            if (playerName.HasValue && guessedNumber.HasValue)
            {
                var player = GetPlayer(game, playerName.Value);
                if (player.HasValue)
                {
                    var guess = new Guess()
                    {
                        Player = player.Value,
                        Estimate = guessedNumber.Value
                    };
                    var actionRes = await TryAction(game.AddGuess(guess));

                    if (actionRes.IsFailure)
                    {
                        return BuildSpeechletResponse("Error", "Schätzung konnte nicht hinzugefügt werden", false);
                    }
                    else
                    {
                        var message = new StringBuilder();
                        message.AppendLine($"{player.Value.Name} schätzt {guessedNumber.Value}");
                        if (game.CurrentRound.GuessesCompleted)
                        {
                            var losers = game.CurrentRound.Losers.Select(x => x.Name).ToList();

                            message.AppendLine($"Die Runde ist beendet. Die korrekte Antwort war: {game.CurrentRound.Puzzle.Answer}");
                            if (losers.Count > 1)
                            {
                                message.AppendLine($"Die Verlierer sind {string.Join(", ", losers)}");
                            }
                            else
                            {
                                message.AppendLine($"Der Verlierer ist {losers.First()}");
                            }
                            message.AppendLine("An die Verlierer: Prost");
                            message.AppendLine("Willst du eine neue Runde starten oder das Spiel beenden");
                        }
                        return BuildSpeechletResponse($"Spieler {player.Value.Name} schätzt {guessedNumber.Value}",
                           message.ToString(), false);
                    }
                }
                else
                {
                    return BuildSpeechletResponse("Kein gültiger Spielername", "Spielername ist nicht im Spiel vorhanden", false);
                }
            }
            else
            {
                if (!playerName.HasValue)
                {
                    return BuildSpeechletResponse("Kein Spielername", "Spielername konnte nicht verstanden werden", false);
                }
                if(!guessedNumber.HasValue)
                {
                    return BuildSpeechletResponse(" Keine Schätzung", "Schätzung konnte nicht verstanden werden", false);
                }
                return BuildSpeechletResponse("Kein Spielername / Keine Schätzung", "Spielername oder Schätzung konnte nicht verstanden werden", false);
            }
        }

        private async Task<SpeechletResponse> FinishPlayers(Intent intent, Game game)
        {
            var actionRes = await TryAction(game.CompleteAddingdPlayers());
            if (actionRes.IsFailure)
            {
                return BuildSpeechletResponse("Error", "Hinzufügen konnte nicht beeendet werden", false);
            }
            else {
                return BuildSpeechletResponse("Alle Spieler hinzugefügt",
                    $"Spieler hinzufügen wurde beendet. Hier eine Frage: {game.CurrentRound.Puzzle.Question}", false);
            }
        }

        private async Task<SpeechletResponse> AddPlayer(Intent intent, Game game)
        {
            var playerName = intent.Slots.TryGetValue(PLAYERNAME_SLOT).Map(x => x.Value);
            if (playerName.HasValue)
            {
                var player = new Player
                {
                    Name = playerName.Value
                };
                var actionRes = await TryAction(game.AddPlayer(player));

                if (actionRes.IsFailure)
                {
                    return BuildSpeechletResponse("Error", "Spieler konnte nicht hinzugefügt werden", false);
                }
                else
                {
                    return BuildSpeechletResponse($"Spieler {playerName.Value}", $"Ich habe {playerName.Value} zum Spiel hinzugefügt", false);
                }
            }
            else
            {
                return BuildSpeechletResponse("Kein Spielername", "Spielername konnte nicht verstanden werden", false);
            }
        }

        public async Task<SpeechletResponse> StartGame(Intent intent, Session session)
        {
            var gameId = _gameService.StartNewGame("de");
            session.Attributes[GAMEID_KEY] = gameId.ToString();
            string message = $"Neues Spiel mit der ID {gameId} wurde gestartet. Füge doch ein paar Spieler hinzu";
            return BuildSpeechletResponse($"Game {gameId}", message, false);
        }

        public override async Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            Log.Info("OnLaunch requestId={0}, sessionId={1}", launchRequest.RequestId, session.SessionId);
            return GetWelcomeResponse();
        }

        public override async Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            Log.Info("OnSessionStarted requestId={0}, sessionId={1}", sessionStartedRequest.RequestId, session.SessionId);
        }

        public override async Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            Log.Info("OnSessionEnded requestId={0}, sessionId={1}", sessionEndedRequest.RequestId, session.SessionId);
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            return BuildSpeechletResponse("Welcome", "Ein herzliches Prost von Poldi", false);
        }

        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            // Create the Simple card content.
            SimpleCard card = new SimpleCard
            {
                Title = $"SessionSpeechlet - {title}",
                Content = $"SessionSpeechlet - {output}"
            };

            // Create the plain text output.
            var lines = output.Split('\n').Select(x => Regex.Replace(x, @"\b(\d+)", AddMarkup,RegexOptions.Multiline));
            var text = string.Join( "<break strength='x-strong'/> ", lines);
            OutputSpeech speech = new SsmlOutputSpeech
            {
                Ssml = $"<speak>{text}</speak>"
            };

            // Create the speechlet response.
            return new SpeechletResponse
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = speech,
                Card = card
            };
        }

        private string AddMarkup(Match match)
        {
            return match.Value.TryParseInt()
                .Match<string>()
                .Some().Do(x => $"<say-as interpret-as='cardinal'>{x}</say-as>")
                .None().Do(match.Value)
                .Result();
        }

        private async Task<Option<Game>> CurrentGame(Session session)
        {
            var gameId = session.Attributes.TryGetValue(GAMEID_KEY)
                .Map(x => x.TryParseInt()).Flatten();

            if (gameId.HasValue)
            {
                var game = _gameService.Games.FirstOrDefault(x => x.Id == gameId);
                return Option<Game>.Some(game);
            }
            return Option<Game>.None();
        }

        private async Task<Success<string>> TryAction(Task action)
        {
            return await action.ToObservable().Select(_ => new Success<string>())
                .Catch<Success<string>, StateException>(exception => Observable.Return(Success.CreateFailure("Dies ist weder der rechte Ort noch die rechte Zeit dafür")));
        }

        private async Task<Either<T, string>> TryAction<T>(Task<T> action)
        {
            return await action.ToObservable().Select(x => new Either<T, string>(x))
                .Catch<Either<T, string>, StateException>(exception => Observable.Return(new Either<T, string>("Dies ist weder der rechte Ort noch die rechte Zeit dafür")));
        }

        private Option<Player> GetPlayer(Game game, string playerName)
        {
            var player = game.Players.TryFirst(x => playerName.ToLower() == x.Name.ToLower() || playerName.ToLower().Contains(x.Name.ToLower()) || x.Name.ToLower().Contains(playerName.ToLower()));
            return player;
        }

        private string WriteScores(Game game)
        {
            return $"Aktuelle Punkte: {string.Join("\n", game.Players.Select(x => $"{x.Name}: {x.Score}"))}";
        }
    }
}