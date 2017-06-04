using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.Shared.DataTransfer;
using DrinkingGame.Shared.Interfaces;
using DrinkingGame.WebService.Hubs;
using DrinkingGame.WebService.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace DrinkingGame.WebService.Communicators
{
    public class DrinkingGameCommunicator : IDrinkingGameCommunicator
    {
        private readonly IHubContext<IGameClient> _hub;

        public DrinkingGameCommunicator(IHubContext<IGameClient> hub)
        {
            _hub = hub;
        }

        public IHubContext<IGameClient> Hub => _hub;

        public void NewAnswer(Game game, string question, string player, int answer)
        {
            Hub.Clients.Clients(game.Devices.Select(x => x.ConnectionId).ToList()).NewAnswer(new NewAnswerDto {
                Question = question,
                Player = player,
                Answer = answer
            });
        }

        public void NewQuestion(Game game, string question)
        {
            Hub.Clients.Clients(game.Devices.Select(x => x.ConnectionId).ToList()).NewRound(new NewRoundDto {
                Question = question
            });
        }

        public void CorrectAnswer(Game game, string question, int answer)
        {
            Hub.Clients.Clients(game.Devices.Select(x => x.ConnectionId).ToList()).CorrectAnswer(new CorrectAnswerDto {
                Question = question,
                Answer = answer
            });
        }
        
        public void ShouldDrink(Game game, IEnumerable<string> players)
        {
            Hub.Clients.Clients(game.Devices.Select(x => x.ConnectionId).ToList()).ShouldDrink(new ShouldDrinkDto() {
                Players = players.ToList()
            });
        }

        public void UpdateGameDetails(Game game)
        {
            Hub.Clients.All.UpdateGameDetails(new UpdateGameDetailsDto
            {
                Players = game.Players.Select(x => x.Name).ToList()
            });
        }

        public void UpdateScores(Game game)
        {
            Hub.Clients.Clients(game.Devices.Select(x => x.ConnectionId).ToList()).UpdateScores(new UpdateScoresDto {
                Scores = game.Players.ToDictionary(x => x.Name, y => y.Score)
            });
        }
    }
}