public class Scripture
{
    public Reference Reference { get; private set; }
    public string Text { get; private set; }
    private HashSet<int> hiddenWordIndices;

    public Scripture(Reference reference, string text)
    {
        Reference = reference;
        Text = text;
        hiddenWordIndices = new HashSet<int>();
    }

    public void Display()
    {
        Console.WriteLine(Reference.ToString());
        var words = Text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (hiddenWordIndices.Contains(i))
            {
                Console.Write("___ "); // Replace hidden words with underscores
            }
            else
            {
                Console.Write(words[i] + " ");
            }
        }
        Console.WriteLine();
    }

    public void HideRandomWords(int numberOfWords)
    {
        var words = Text.Split(' ');
        Random random = new Random();

        for (int i = 0; i < numberOfWords; i++)
        {
            int index;
            do
            {
                index = random.Next(words.Length);
            } while (hiddenWordIndices.Contains(index));

            hiddenWordIndices.Add(index);
        }
    }
}
