using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameClient
    {
        void UpdateGameDetails(IEnumerable<string> players);
        void NewQuestion(string question);
        void NewAnswer(string question, string player);
        void CorrectAnswer(string question, string answer);
        void ShouldDrink(string player);
        void UpdateScores(Dictionary<string,int> scores);
    }
}
