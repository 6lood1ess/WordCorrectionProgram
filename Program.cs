using System;
using System.IO;

namespace WordCorrector {
  class Program {
    static void Main() {

      Console.WriteLine("-=*= Text error corrector =*=-\n");

      DictionaryService dictionaryService = new DictionaryService();
      TextCorrector textCorrector = new TextCorrector(dictionaryService);
      FileProcessor fileProcessor = new FileProcessor(textCorrector);

      Console.WriteLine("Select mode:\n" +
                        "1 - Process all .txt files in directory\n" +
                        "2 - Process a single file\n");

      Console.Write("Your choice: ");
      string userChoice = Console.ReadLine();

      if (userChoice == "1") {

        Console.Write("\nEnter directory path: ");
        string directory = Console.ReadLine();
                
        if (Directory.Exists(directory)) {
          fileProcessor.ProcessAllTxtFiles(directory);
        } else {
          Console.WriteLine("Directory not found!");
        }
      } else if (userChoice == "2") {

        Console.Write("\nEnter file path: ");
        string filePath = Console.ReadLine();
                
        if (File.Exists(filePath)) {
          fileProcessor.ProcessSingleFile(filePath);
        } else {
          Console.WriteLine("File not found!");
        }
      } else {
        Console.WriteLine("\nInvalid choice!");
      }

      Console.WriteLine("\nPress any key to exit...");
      Console.ReadKey();
    }
  }
}