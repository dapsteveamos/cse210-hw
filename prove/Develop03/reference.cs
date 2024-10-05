public class Reference
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int VerseStart { get; private set; }
    public int VerseEnd { get; private set; } // Optional

    // Constructor for single verse
    public Reference(string reference)
    {
        var parts = reference.Split(' ');
        Book = parts[0];
        Chapter = int.Parse(parts[1].Split(':')[0]);
        var verseParts = parts[1].Split(':')[1].Split('-');
        VerseStart = int.Parse(verseParts[0]);
        VerseEnd = verseParts.Length > 1 ? int.Parse(verseParts[1]) : VerseStart;
    }

    // Constructor for verse range
    public Reference(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        VerseStart = verse;
        VerseEnd = verse;
    }

    // Constructor for verse range
    public Reference(string book, int chapter, int verseStart, int verseEnd)
    {
        Book = book;
        Chapter = chapter;
        VerseStart = verseStart;
        VerseEnd = verseEnd;
    }

    public override string ToString()
    {
        return VerseEnd > VerseStart ? $"{Book} {Chapter}:{VerseStart}-{VerseEnd}" : $"{Book} {Chapter}:{VerseStart}";
    }
}
