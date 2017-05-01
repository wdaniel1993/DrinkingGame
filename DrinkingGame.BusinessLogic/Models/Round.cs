using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Round
    {
        private readonly Subject<Guess> _guessAdded = new Subject<Guess>();
        private readonly Subject<Unit> _roundCompleted = new Subject<Unit>();
        private readonly Subject<Unit> _drinkTaken = new Subject<Unit>();
        private readonly List<Guess> _guesses = new List<Guess>();
        private bool _isCompleted;
        private bool _isLastRound;

        public bool IsCompleted => _isCompleted;
        public bool IsLastRound => _isLastRound;

        public Puzzle Puzzle { get; set; }
        public IEnumerable<Guess> Guesses => _guesses.AsReadOnly();
        public IObservable<Guess> GuessesAdded => _guessAdded.AsObservable();
        public IObservable<Unit> RoundCompleted => _roundCompleted.AsObservable();
        public IObservable<Unit> DrinkTaken => _drinkTaken.AsObservable();
        public Game Game { get; set; }

        public async Task AddGuess(Guess guess)
        {
            await Game.CheckState(typeof(AnswerReading));
            _guesses.Add(guess);
            _guessAdded.OnNext(guess);

            if (_guesses.Count == Game.Players.Count())
            {
                _guessAdded.OnCompleted();
            }
        }

        public async Task LoserDrank()
        {
            await Game.CheckState(typeof(LoserDrinking));
            _drinkTaken.OnNext(Unit.Default);
        }

        public async Task CompleteRound(bool isLastRound)
        {
            await Game.CheckState(typeof(RoundEnding));
            _isCompleted = true;
            _isLastRound = isLastRound;
            _roundCompleted.OnNext(Unit.Default);
        }
    }
}
