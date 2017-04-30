﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaReader.Core.Manga;
using MangaReader.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Entities.Download
{
  [TestClass]
  public class ReadmangaDL
  {
    private int lastPercent = 0;

    [TestMethod]
    public async Task DownloadReadmanga()
    {
      // var rm = Mangas.Create(new Uri(@"http://henchan.me/related/15692-sweet-guy-glava-0-prolog.html"));
      var rm = Mangas.Create(new Uri(@"http://readmanga.me/hack__xxxx"));
      var sw = new Stopwatch();
      sw.Start();
      rm.DownloadProgressChanged += RmOnDownloadProgressChanged;
      await rm.Download();
      sw.Stop();
      Log.Add($"manga loaded {sw.Elapsed.TotalSeconds}, iscompleted = {rm.IsDownloaded}, lastpercent = {lastPercent}");
      Assert.IsTrue(Directory.Exists(rm.GetAbsoulteFolderPath()));
      var files = Directory.GetFiles(rm.GetAbsoulteFolderPath(), "*", SearchOption.AllDirectories);
      Assert.AreEqual(249, files.Length);
      var fileInfos = files.Select(f => new FileInfo(f)).ToList();
      Assert.AreEqual(64025297, fileInfos.Sum(f => f.Length));
      Assert.IsTrue(rm.Volumes.Sum(v => v.Chapters.Count) > fileInfos.GroupBy(f => f.Length).Max(g => g.Count()));
      Assert.IsTrue(rm.IsDownloaded);
      Assert.AreEqual(100, lastPercent);
    }

    private void RmOnDownloadProgressChanged(object sender, IManga manga)
    {
      var dl = (int) manga.Downloaded;
      if (dl > lastPercent)
        lastPercent = dl;
    }
  }
}