using DrinkingGame.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkingGame.Alexa.Communicators
{
    public interface IDrinkingGameCommunicator
    {
        void UpdateGameDetails(Game game);
        void NewQuestion(Game game, string question);
        void NewAnswer(Game game, string question, string player);
        void ShouldDrink(string player);
        void UpdateScores(Game game);
    }
}