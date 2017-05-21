using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.Shared.DataTransfer;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameServer
    {
        Task ConnectToGame(ConnectToGameDto dto);
        Task PlayerDrank(PlayerDrankDto dto);
        Task GaveAnswer(GaveAnswerDto dto);
    }
}
