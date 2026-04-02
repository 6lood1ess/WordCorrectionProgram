using System.Text;
using System.Text.RegularExpressions;

namespace WordCorrector {
  public class TextCorrector {

    public DictionaryService DictionaryService;

    // Regular expression for finding phone numbers in format (XXX) XXX-XX-XX
    public static Regex PhoneRegex = new Regex(
      @"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})",
      RegexOptions.Compiled
    );

    public TextCorrector(DictionaryService dictionaryService) {
      DictionaryService = dictionaryService;
    }

    public string CorrectText(string inputText) {

      if (string.IsNullOrWhiteSpace(inputText)) {
        return inputText;
      }

      // Stage 1: correct spelling errors
      string correctedText = CorrectSpellingErrors(inputText);

      // Stage 2: replace phone numbers
      correctedText = ReplacePhoneNumbers(correctedText);

      return correctedText;
    }

    public string CorrectSpellingErrors(string text) {
      // Split text into words while preserving separators
      string[] words = Regex.Split(text, @"(\s+|[.,!?;:()\[\]{}""'<>@#№$%^&*\-+=/\\|])");

      StringBuilder result = new StringBuilder();

      for (int wordIndex = 0; wordIndex < words.Length; ++wordIndex) {
        string currentWord = words[wordIndex];

        // Check if the token is a word (only letters)
        if (Regex.IsMatch(currentWord, @"^[а-яА-Яa-zA-Z]+$")) {
          string correctedWord = DictionaryService.CorrectWord(currentWord);
          result.Append(correctedWord);
        } else {
          // Preserve separators and punctuation as is
          result.Append(currentWord);
        }
      }

      return result.ToString();
    }

    public string ReplacePhoneNumbers(string text) {
      return PhoneRegex.Replace(text, new MatchEvaluator(ReplacePhoneMatch));
    }

    public string ReplacePhoneMatch(Match match) {
      // Extract phone parts using Match.Groups
      string cityCode = match.Groups[1].Value;
      string subscriberPrefix = match.Groups[2].Value;
      string subscriberMiddle = match.Groups[3].Value;
      string subscriberSuffix = match.Groups[4].Value;

      // Remove leading zero from city code
      string shortCode;
      if (cityCode.StartsWith("0")) {
        shortCode = cityCode.Substring(1);
      } else {
        shortCode = cityCode;
      }

      StringBuilder builder = new StringBuilder("+380 ");
      builder.Append(shortCode).Append(" ");
      builder.Append(subscriberPrefix).Append(" ");
      builder.Append(subscriberMiddle).Append(" ");
      builder.Append(subscriberSuffix);

      return builder.ToString();
    }
  }
}
