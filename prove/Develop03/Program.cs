// Exceeding Limit: I added Timer Feature: The program tracks the time taken to complete the scripture memorization by starting a stopwatch at the beginning and stopping it when all words are hidden. The time is then displayed to the user.
// Word Reveal Feature: The user can type "reveal" to show one random hidden word if they need help. This allows them to gradually memorize the scripture with some assistance.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; // For the timer this is so that i can exceed limit
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Start the timer
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Scripture reference and text
        Reference reference = new Reference("Proverbs", 3, 5, 6);
        Scripture scripture = new Scripture(reference, "Trust in the Lord with all thine heart and lean not unto thine own understanding. In all thy ways acknowledge him and he shall direct thy paths.");

        // Keep the program running until all words are hidden or the user types "quit"
        while (!scripture.AllWordsHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetRenderedText());
            Console.WriteLine("\nPress Enter to hide words, type 'reveal' to show a word, or type 'quit' to exit.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
                break;
            else if (input.ToLower() == "reveal")
                scripture.RevealRandomWord();  // Reveals one random word
            else
                scripture.HideRandomWords(3);  // Hide random words
        }

        // Stop the timer and display the elapsed time
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        Console.Clear();
        Console.WriteLine(scripture.GetRenderedText());
        Console.WriteLine($"\nAll words have been hidden. You completed the task in {ts.Minutes} minutes and {ts.Seconds} seconds.");
    }
}

class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public void Reveal()
    {
        _isHidden = false;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetRenderedText()
    {
        return _isHidden ? "____" : _text;
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public string GetReference()
    {
        if (_startVerse == _endVerse)
        {
            return $"{_book} {_chapter}:{_startVerse}";
        }
        else
        {
            return $"{_book} {_chapter}:{_startVerse}-{_endVerse}";
        }
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public string GetRenderedText()
    {
        string referenceText = _reference.GetReference();
        string scriptureText = string.Join(" ", _words.Select(word => word.GetRenderedText()));
        return $"{referenceText} {scriptureText}";
    }

    public void HideRandomWords(int count)
    {
        Random random = new Random();
        int hiddenCount = 0;

        while (hiddenCount < count)
        {
            // Select a random word that is not already hidden
            List<Word> visibleWords = _words.Where(word => !word.IsHidden()).ToList();
            if (visibleWords.Count == 0) break; // No more visible words to hide

            int index = random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            hiddenCount++;
        }
    }

    public void RevealRandomWord()
    {
        Random random = new Random();
        // Select a random word that is hidden
        List<Word> hiddenWords = _words.Where(word => word.IsHidden()).ToList();
        if (hiddenWords.Count > 0)
        {
            int index = random.Next(hiddenWords.Count);
            hiddenWords[index].Reveal();
        }
    }

    public bool AllWordsHidden()
    {
        return _words.All(word => word.IsHidden());
    }
}
