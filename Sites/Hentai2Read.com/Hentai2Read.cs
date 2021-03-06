﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangaReader.Core;
using MangaReader.Core.Services;

namespace Hentai2Read.com
{
  /// <summary>
  /// Манга.
  /// </summary>
  public class Hentai2Read : MangaReader.Core.Manga.Mangas
  {
    public override List<Compression.CompressionMode> AllowedCompressionModes
    {
      get { return base.AllowedCompressionModes.Where(m => !Equals(m, Compression.CompressionMode.Chapter)).ToList(); }
    }

    public override bool HasVolumes { get { return false; } }

    public override bool HasChapters { get { return true; } }

    protected override async Task CreatedFromWeb(Uri url)
    {
      await base.CreatedFromWeb(url).ConfigureAwait(false);

      if (this.Uri != url && Parser.ParseUri(url).Kind != UriParseKind.Manga)
      {
        await this.UpdateContent().ConfigureAwait(false);

        AddHistoryReadedUris(this.Chapters, url);
      }
    }
  }
}
