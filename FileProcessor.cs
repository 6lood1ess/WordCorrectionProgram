using System;
using System.Text;
using System.IO;

namespace WordCorrector {
  public class FileProcessor {

    public TextCorrector TextCorrector;

    public FileProcessor(TextCorrector textCorrector) {
      TextCorrector = textCorrector;
    }

    public void ProcessAllTxtFiles(string directory) {

      string[] txtFiles = Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories);

      if (txtFiles.Length == 0) {
        Console.WriteLine("\nNo .txt files found for processing.");
        return;
      }

      Console.WriteLine($"\nFound {txtFiles.Length} files to process:\n");

      for (int fileIndex = 0; fileIndex < txtFiles.Length; ++fileIndex) {
        string currentFilePath = txtFiles[fileIndex];
        ProcessSingleFile(currentFilePath);
      }
    }

    public void ProcessSingleFile(string filePath) {

      Console.WriteLine($"Processing: {filePath}");

      string originalContent, correctedContent;

      try {

        originalContent = File.ReadAllText(filePath, Encoding.UTF8);
        correctedContent = TextCorrector.CorrectText(originalContent);

        if (originalContent == correctedContent) {
          Console.WriteLine("-  No changes needed\n");
          return;
        }

        File.WriteAllText(filePath, correctedContent, Encoding.UTF8);
        Console.WriteLine("-  File successfully corrected :)\n");

      } catch (UnauthorizedAccessException) {
        Console.WriteLine("-  Error: access denied to file!\n");

      } catch (IOException exception) {
        Console.WriteLine($"-  IO Error: {exception.Message}\n");

      } catch (Exception exception) {
        Console.WriteLine($"-  Unexpected error: {exception.Message}\n");
      }
    }
  }
}