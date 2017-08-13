﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grouple;
using MangaReader.Core.Manga;
using NUnit.Framework;

namespace Tests.Entities.Search
{
  [TestFixture]
  public class Grouple : TestClass
  {
    private Parser parser = new Parser();
    
    [Test]
    public void Search()
    {
      var chapters = Search("my name");
      Assert.AreEqual(0, chapters);
    }

    private IEnumerable<IManga> Search(string name)
    {
      return parser.Search(name);
    }

  }
}