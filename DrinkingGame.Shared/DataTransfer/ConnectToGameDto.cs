using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.DataTransfer
{
    public class ConnectToGameDto
    {
        public int GameNumber { get; set; }
        public bool SupportShouldDrink { get; set; }
    }
}
