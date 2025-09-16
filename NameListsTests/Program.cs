using NameLists;
using NameLists.DataStructures;

namespace NameListsTests;

class Program
{
    static void Main(string[] args)
    {
        string testFileName = "/Users/oleg/Desktop/S1REF1000W.txt";
        const string testNameList = """
                              #Test NameList Title  
                              &Block1
                                  F1 = 1,
                                  F2 = -1,
                                  F3 = 1.3,
                                  F4 = -1.3,
                              &END
                              """;
        
        string rawText = Helpers.TextFileReader.ReadTextFromFile(testFileName);
        //string rawText = testNameList;
        
        NameList list = NameList.CreateFromText(rawText);
        
        var testString = list.ToString();
        
        Console.WriteLine(testString);
    }
}