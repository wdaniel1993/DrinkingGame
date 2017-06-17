using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.WebService.Services
{
    public interface IPuzzleService
    {
        Puzzle GetRandomPuzzle(string language);

        IEnumerable<Puzzle> Puzzles(string language);
    }
}