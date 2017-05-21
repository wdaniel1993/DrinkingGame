using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.DataTransfer
{
    public class UpdateGameDetailsDto
    {
        public IEnumerable<string> Players { get; set; }
    }
}
