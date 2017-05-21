using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.DataTransfer
{
    public class NewAnswerDto
    {
        public string Question { get; set; }
        public string Player { get; set; }
        public int Answer { get; set; }
    }
}
