using System;
using System.Collections.Generic;
using System.Linq;

namespace HangmanGame
{
    public interface IGameState
    {
        void DisplayWord();
        HangmanGameState GuessLetter(char letter);
        bool IsGameOver();
        bool IsWordGuessed();
    }

    // The primary constructor ensures that the game state can be properly initialized.
    public record HangmanGameState(string WordToGuess, HashSet<char> GuessedLetters, HashSet<char> IncorrectGuesses, int RemainingTries) : IGameState
    {
        public void DisplayWord()
        {
            var display = WordToGuess.Select(c => GuessedLetters.Contains(c) ? c : '_').ToArray();
            Console.WriteLine(new string(display));
        }

        public HangmanGameState GuessLetter(char letter)  // Return a new instance with updated state
        {
            if (WordToGuess.Contains(letter))
            {
                GuessedLetters.Add(letter);
            }
            else
            {
                IncorrectGuesses.Add(letter);
                // Create a new instance with the updated RemainingTries
                return this with { RemainingTries = RemainingTries - 1 };
            }
            return this;  // If no changes, return the same instance
        }

        public bool IsGameOver() => RemainingTries <= 0 || IsWordGuessed();
        public bool IsWordGuessed() => WordToGuess.All(c => GuessedLetters.Contains(c));
    }

    public class HangmanGame
    {
        private HangmanGameState _gameState;

        public HangmanGame(string wordToGuess, int maxTries = 6)
        {
            // Initializing the game state properly with a given word and max tries.
            _gameState = new HangmanGameState(wordToGuess, new HashSet<char>(), new HashSet<char>(), maxTries);
        }

        public void Play()
        {
            while (!_gameState.IsGameOver())
            {
                Console.Clear();
                Console.WriteLine($"Remaining Tries: {_gameState.RemainingTries}");
                _gameState.DisplayWord();
                Console.WriteLine("Incorrect guesses: " + string.Join(", ", _gameState.IncorrectGuesses));
                Console.Write("Enter a letter: ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input != null && input.Length == 1 && char.IsLetter(input[0]))
                {
                    _gameState = _gameState.GuessLetter(input[0]);  // Update _gameState with the new instance
                }
                else
                {
                    Console.WriteLine("Please enter a valid letter.");
                }
            }

            Console.Clear();
            _gameState.DisplayWord();

            if (_gameState.IsWordGuessed())
            {
                Console.WriteLine("Congratulations! You've guessed the word.");
            }
            else
            {
                Console.WriteLine("Game Over! You've run out of tries.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman!");
            Console.Write("Enter the word to guess: ");
            var wordToGuess = Console.ReadLine()?.Trim().ToLower();

            if (!string.IsNullOrEmpty(wordToGuess))
            {
                var game = new HangmanGame(wordToGuess);
                game.Play();
            }
            else
            {
                Console.WriteLine("Invalid input. Exiting...");
            }
        }
    }
}
