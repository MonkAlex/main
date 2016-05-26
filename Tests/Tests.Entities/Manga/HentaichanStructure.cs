﻿using System;
using System.Linq;
using MangaReader.Core.Manga;
using MangaReader.Core.Manga.Hentaichan;
using MangaReader.Core.Services.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Entities.Manga
{
  [TestClass]
  public class HentaichanStructure
  {
    [TestMethod]
    public void AddHentaichanMultiPages()
    {
      var manga = GetManga("http://hentaichan.me/related/14212-love-and-devil-glava-25.html");
      Assert.AreEqual(25, manga.Chapters.Count);
      Assert.IsTrue(manga.HasChapters);
    }

    private Hentaichan GetManga(string url)
    {
      CreateLogin();
      var manga = Mangas.Create(new Uri(url)) as Hentaichan;
      Getter.UpdateContent(manga);
      return manga;
    }

    private void CreateLogin()
    {
      var setting = ConfigStorage.Instance.DatabaseConfig.MangaSettings.Single(s => Equals(s.Manga, Hentaichan.Type));
      var login = setting.Login as HentaichanLogin;
      login.UserId = "235332";
      login.PasswordHash = "0578caacc02411f8c9a1a0af31b3befa";
      login.IsLogined = true;
      setting.Save();
    }
  }
}