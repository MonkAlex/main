using System;
using System.Linq;
using System.Threading.Tasks;
using MangaReader.Core.Manga;
using NUnit.Framework;
using MangaReader.Core.Services.Config;

namespace Tests.Entities.Manga
{
  [TestFixture]
  public class MangachanStructure : TestClass
  {
    [Test]
    public async Task AddMangachanMultiPages()
    {
      var manga = await GetManga("http://mangachan.me/manga/15659-this-girlfriend-is-fiction.html");
      await new Hentaichan.Mangachan.Parser().UpdateContent(manga);
      Assert.AreEqual(4, manga.Volumes.Count);
      Assert.AreEqual(34, manga.Volumes.Sum(v => v.Container.Count()));
    }

    [Test]
    public async Task AddMangachanSingleChapter()
    {
      var manga = await GetManga("http://mangachan.me/manga/20138-16000-honesty.html");
      await new Hentaichan.Mangachan.Parser().UpdateContent(manga);
      Assert.AreEqual(1, manga.Volumes.Count);
      Assert.AreEqual(1, manga.Volumes.Sum(v => v.Container.Count()));
    }

    private static async Task<IManga> GetManga(string url)
    {
      return await Mangas.CreateFromWeb(new Uri(url));
    }

    [Test]
    public async Task MangachanNameParsing()
    {
      // Спецсимвол "
      await TestNameParsing("http://mangachan.me/manga/48069-isekai-de-kuro-no-iyashi-te-tte-yobarete-imasu.html",
        "Isekai de \"Kuro no Iyashi Te\" tte Yobarete Imasu",
        "В другом мире моё имя - Чёрный целитель");

      // Просто проверка.
      await TestNameParsing("http://mangachan.me/manga/46475-shin5-kekkonshite-mo-koishiteru.html",
        "#shin5 - Kekkonshite mo Koishiteru",
        "Любовь после свадьбы");

      // Нет русского варианта.
      await TestNameParsing("http://mangachan.me/manga/17625--okazaki-mari.html",
        "& (Okazaki Mari)",
        "& (Okazaki Mari)");

      // Символ звездочки *
      await TestNameParsing("http://mangachan.me/manga/23099--asterisk.html",
        "* - Asterisk",
        "Звездочка");
    }

    private async Task TestNameParsing(string uri, string english, string russian)
    {
      ConfigStorage.Instance.AppConfig.Language = Languages.English;
      var manga = await GetManga(uri);
      Assert.AreEqual(english, manga.Name);
      ConfigStorage.Instance.AppConfig.Language = Languages.Russian;
      await new Hentaichan.Mangachan.Parser().UpdateNameAndStatus(manga);
      Assert.AreEqual(russian, manga.Name);
    }
  }
}
