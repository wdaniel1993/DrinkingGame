using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.BusinessLogic.Models
{
    [Serializable]
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
