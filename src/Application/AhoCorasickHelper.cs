using System.Reflection;
using System.Text;
using System.Text.Json;
using Ganss.Text;

namespace Nexus.Application.Extensions;

public static class AhoCorasickHelper
{
    /// <summary>
    /// Checks the <paramref name="str"/> for banned words using Aho-Corasick trie algorithm and replaces them with arterisks ('*')
    /// </summary>
    /// <param name="str">Input string to be searched</param>
    /// <param name="jsonFilePath">Path to JSON file with the wordlist</param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task ReplaceBannedWordsFor<TEntity>(TEntity entity, string relativePath) where TEntity : class
    {
        foreach (PropertyInfo prop in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string)))
        {
            string value = prop.GetValue(entity)!.ToString()!;

            string path = Directory.GetCurrentDirectory() + relativePath;
            FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + relativePath);
            WordBag? wordBag = await JsonSerializer.DeserializeAsync<WordBag>(fs);
            fs.Close();

            if (wordBag == null) 
                throw new ArgumentException("The input JSON file was not in the correct format!");

            StringBuilder sb = new StringBuilder(value.Length);
            int index = 0;
            foreach (WordMatch match in value.Contains(wordBag.Words))
            {
                if (match.Index + match.Word.Length < index) continue;

                sb.Append(value.Substring(index, match.Index - index));
                sb.Append("".PadLeft(match.Word.Length, '*'));
                
                index = match.Index + match.Word.Length;
            }
            if (index < value.Length) 
            {
                sb.Append(value.Substring(index));
            }

            prop.SetValue(entity, sb.ToString());
        }
    }

    private record WordBag(IEnumerable<string> Words);
}