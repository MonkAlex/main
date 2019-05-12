﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MangaReader.Core.Account;
using MangaReader.Core.Convertation.Primitives;
using MangaReader.Core.NHibernate;
using MangaReader.Core.Services;

namespace MangaReader.Core.Convertation.Config
{
  public class From47To48 : ConfigConverter
  {
    protected override async Task ProtectedConvert(IProcess process)
    {
      using (var context = Repository.GetEntityContext())
      {
        var proxySetting = await context
          .Get<ProxySetting>()
          .Where(s => s.SettingType == ProxySettingType.System)
          .SingleAsync().ConfigureAwait(false);
        var settings = await context.Get<MangaSetting>().Where(s => s.ProxySetting == null).ToListAsync().ConfigureAwait(false);
        foreach (var setting in settings)
        {
          setting.ProxySetting = proxySetting;
        }

        await settings.SaveAll(context).ConfigureAwait(false);

        await MangaSettingCache.RevalidateCache().ConfigureAwait(false);
      }
    }

    public From47To48()
    {
      this.Name = "Поддержка прокси";
      this.Version = new Version(1, 48, 1);
    }
  }
}
