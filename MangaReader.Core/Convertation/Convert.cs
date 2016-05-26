﻿using System;
using System.Collections.Generic;
using System.Linq;
using MangaReader.Core.Convertation.Primitives;
using MangaReader.Core.Services;
using MangaReader.Core.Services.Config;

namespace MangaReader.Core.Convertation
{

  public static class Converter
  {
    static public void Convert(IProcess process)
    {
      process.State = ConvertState.Started;

      Log.Add("Convert started.");

      Convert<ConfigConverter>(process);

      Log.AddFormat("Found {0} manga type settings:", ConfigStorage.Instance.DatabaseConfig.MangaSettings.Count);
      ConfigStorage.Instance.DatabaseConfig.MangaSettings.ForEach(s => Log.AddFormat("Load settings for {0}, guid {1}.", s.MangaName, s.Manga));

      Convert<MangasConverter>(process);
      Convert<DatabaseConverter>(process);
      Convert<HistoryConverter>(process);

      Log.Add("Convert completed.");

      ConfigStorage.Instance.DatabaseConfig.Version = process.Version;
      process.State = ConvertState.Completed;
    }

    private static void Convert<T>(IProcess process) where T : BaseConverter
    {
      var converters = new List<T>();
      foreach (var assembly in ResolveAssembly.AllowedAssemblies())
      {
        converters.AddRange(assembly.GetTypes()
          .Where(t => !t.IsAbstract && t.IsClass && typeof(T).IsAssignableFrom(t))
          .Select(Activator.CreateInstance)
          .OfType<T>());
      }
      converters = converters.OrderBy(c => c.Version).ToList();
      foreach (var converter in converters)
      {
        process.Status = converter.Name;
        process.Percent = 0;
        process.ProgressState = converter.CanReportProcess ? ProgressState.Normal : ProgressState.Indeterminate;
        converter.Convert(process);
      }
    }
  }

  public enum ConvertState
  {
    None,
    Started,
    Completed
  }

  /// <summary>
  /// Определяет состояние индикатора хода выполнения на панели задач Windows.
  /// </summary>
  public enum ProgressState
  {
    None,
    Indeterminate,
    Normal,
    Error,
    Paused,
  }
}