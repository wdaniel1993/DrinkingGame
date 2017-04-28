using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Round
    {
        private readonly Subject<Guess> _guessAdded = new Subject<Guess>();
        private readonly List<Guess> _guesses = new List<Guess>();

        public bool Complete { get; set; }
        public Puzzle Puzzle { get; set; }
        public IEnumerable<Guess> Guesses => _guesses.AsReadOnly();
        public IObservable<Guess> GuessesAdded => _guessAdded.AsObservable();
        public IObservable<int> RoundCompleted => _guessAdded.Count();

        public void AddGuess(Guess guess)
        {
            _guesses.Add(guess);
            _guessAdded.OnNext(guess);
        }

        public void CompleteRound()
        {
            Complete = true;
            _guessAdded.OnCompleted();
        }
    }
}
