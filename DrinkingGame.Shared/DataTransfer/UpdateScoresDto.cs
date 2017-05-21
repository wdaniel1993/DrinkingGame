using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.DataTransfer
{
    public class UpdateScoresDto
    {
        public Dictionary<string, int> Scores { get; set; }
    }
}
