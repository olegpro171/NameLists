using NameLists;

namespace NameListsTests;

class Program
{
    static void Main(string[] args)
    {
        string testFileName = "/Users/oleg/Desktop/S1REF1000W.txt";
        const string testNameList = """
                              #Test NameList Title  
                              &Block1
                                  F1 = 5*1.0,
                                  F2 = 1, 2, 3, 4, 4 ,4 ,4,
                                  F3 = 'abcdefg', 'abcdefg',
                                  F4 = 1, 1.2, 4, 'abcdef',
                              &END
                              """;
        
        //string rawText = Helpers.TextFileReader.ReadTextFromFile(testFileName);
        string rawText = testNameList;
        
        NameList list = NameLists.Factories.NameListFactory.CreateFromText(rawText);
        
        var testString = list.ToString();
        
        Console.WriteLine(testString);
    }
}