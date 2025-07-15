using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HarryPotterMcpServer.Tools;

[McpServerToolType]
public static class HPTools
{
  [McpServerTool, Description("Get a list of Harry Potter books")]
  public static async Task<string> GetBooks(
      HttpClient client,
      [Description("Language of the books. Default is English (en).")] Language language = Language.en
      )
  {
    var url = string.Format("/{0}/books", language.ToString());

    using var jsonDocument = await client.ReadJsonDocumentAsync(url);
    var books = jsonDocument.RootElement.EnumerateArray();

    if(!books.Any())
    {
      return "No books found.";
    }

    return string.Join(Environment.NewLine, books.Select(book =>
          {
          return $"""
          Title: {book.GetProperty("title").GetString()}
          Original Title: {book.GetProperty("originalTitle").GetString()}
          Number: {book.GetProperty("number").GetInt64()}
          Release Date: {book.GetProperty("releaseDate").GetString()}
          Description: {book.GetProperty("description").GetString()}
          Pages: {book.GetProperty("pages").GetInt64()}
          Cover: {book.GetProperty("cover").GetString()}
          """;
          }));
  }

  [McpServerTool, Description("Get a random Harry Potter book")]
  public static async Task<string> GetRandomBook(
      HttpClient client,
      [Description("Language of the book. Default is English (en).")] Language language = Language.en
      )
  {
    var url = string.Format("/{0}/books/random", language.ToString());

    using var jsonDocument = await client.ReadJsonDocumentAsync(url);
    var book = jsonDocument.RootElement;

    return $"""
    Title: {book.GetProperty("title").GetString()}
    Original Title: {book.GetProperty("originalTitle").GetString()}
    Number: {book.GetProperty("number").GetInt64()}
    Release Date: {book.GetProperty("releaseDate").GetString()}
    Description: {book.GetProperty("description").GetString()}
    Pages: {book.GetProperty("pages").GetInt64()}
    Cover: {book.GetProperty("cover").GetString()}
    """;
  }

  [McpServerTool, Description("Get a list of Harry Potter characters")]
  public static async Task<string> GetCharacters(
      HttpClient client,
      [Description("Language of the characters. Default is English (en).")] Language language = Language.en,
      [Description("Searches all items and returns the best matches")] String? search = null
      )
  {
    var url = string.Format("/{0}/characters", language.ToString());

    if(!string.IsNullOrEmpty(search))
    {
      url += $"?search={Uri.EscapeDataString(search)}";
    }

    using var jsonDocument = await client.ReadJsonDocumentAsync(url);
    var characters = jsonDocument.RootElement.EnumerateArray();

    if(!characters.Any())
    {
      return "No characters found.";
    }

    return string.Join(Environment.NewLine, characters.Select(character =>
          {
          return $"""
          FullName: {character.GetProperty("fullName").GetString()}
          NickName: {character.GetProperty("nickName").GetString()}
          HogwartsHouse: {character.GetProperty("hogwartsHouse").GetString()}
          InterpretedBy: {character.GetProperty("interpretedBy").GetString()}
          Children: {string.Join(", ", character.GetProperty("children").EnumerateArray().Select(c => c.GetString()))}
          Image: {character.GetProperty("image").GetString()}
          Birthdate: {character.GetProperty("birthdate").GetString()}
          """;
          }));
  }

  [McpServerTool, Description("Get a random Harry Potter character")]
  public static async Task<string> GetRandomCharacter(
      HttpClient client,
      [Description("Language of the character. Default is English (en).")] Language language = Language.en
      )
  {
    var url = string.Format("/{0}/characters/random", language.ToString());

    using var jsonDocument = await client.ReadJsonDocumentAsync(url);
    var character = jsonDocument.RootElement;

    return $"""
    FullName: {character.GetProperty("fullName").GetString()}
    NickName: {character.GetProperty("nickName").GetString()}
    HogwartsHouse: {character.GetProperty("hogwartsHouse").GetString()}
    InterpretedBy: {character.GetProperty("interpretedBy").GetString()}
    Children: {string.Join(", ", character.GetProperty("children").EnumerateArray().Select(c => c.GetString()))}
    Image: {character.GetProperty("image").GetString()}
    Birthdate: {character.GetProperty("birthdate").GetString()}
    """;
  }
}

public enum Language
{
  [Description("English")]
  en,
  [Description("Spanish")]
  es,
  [Description("French")]
  fr,
  [Description("Italian")]
  it,
  [Description("Portuguese")]
  pt,
  [Description("Ukrainian")]
  uk
}
