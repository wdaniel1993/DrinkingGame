using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.Shared.DataTransfer;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameClient
    {
        void UpdateGameDetails(UpdateGameDetailsDto dto);
        void NewRound(NewRoundDto dto);
        void NewAnswer(NewAnswerDto dto);
        void CorrectAnswer(CorrectAnswerDto dto);
        void ShouldDrink(ShouldDrinkDto dto);
        void UpdateScores(UpdateScoresDto dto);
    }
}
