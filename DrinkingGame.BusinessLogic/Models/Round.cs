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
        private readonly Subject<IEnumerable<Player>> _roundCompleted = new Subject<IEnumerable<Player>>();
        private readonly Subject<Player> _drinkTaken = new Subject<Player>();
        private readonly List<Guess> _guesses = new List<Guess>();
        private readonly List<Player> _losersDrank = new List<Player>();
        private bool _isCompleted;
        private bool _isLastRound;

        public bool IsCompleted => _isCompleted;
        public bool IsLastRound => _isLastRound;

        public Puzzle Puzzle { get; set; }
        public IEnumerable<Guess> Guesses => _guesses.AsReadOnly();
        public IEnumerable<Player> LosersDrank => _losersDrank.AsReadOnly();
        public IObservable<Guess> GuessesAdded => _guessAdded.AsObservable();
        public IObservable<IEnumerable<Player>> RoundCompleted => _roundCompleted.AsObservable();
        public IObservable<Player> DrinkTaken => _drinkTaken.AsObservable();
        public Game Game { get; set; }

        public IEnumerable<Player> Losers
        {
            get { return Guesses.MaxBy(x => Math.Abs(x.Estimate - Puzzle.Answer)).Select(x => x.Player); }
        }

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

        public async Task PlayerDrank(Player player)
        {
            await Game.CheckState(typeof(LoserDrinking));
            if (Losers.Contains(player) && !_losersDrank.Contains(player))
            {
                _losersDrank.Add(player);
                _drinkTaken.OnNext(player);
            }
            if (_losersDrank.Count == Losers.Count())
            {
                _drinkTaken.OnCompleted();
            }
        }

        public async Task CompleteRound(bool isLastRound)
        {
            await Game.CheckState(typeof(RoundEnding));
            Losers.ForEach(x => x.Score++);
            _isCompleted = true;
            _isLastRound = isLastRound;
            _roundCompleted.OnNext(Losers);
        }
    }
}
