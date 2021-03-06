﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MangaReader.Core.Services;
using MangaReader.Core.Services.Config;

namespace MangaReader.Core.NHibernate
{
  public static class Repository
  {
    public static RepositoryContext GetEntityContext([CallerMemberName] string name = null)
    {
      return RepositoryContext.Create(name ?? Guid.NewGuid().ToString("D"));
    }

    public static T GetStateless<T>(int id) where T : Entity.IEntity
    {
      using (var session = Mapping.GetStatelessSession())
      {
        return session.Get<T>(id);
      }
    }

    public static List<T> GetStateless<T>() where T : Entity.IEntity
    {
      using (var session = Mapping.GetStatelessSession())
      {
        return session.Query<T>().ToList();
      }
    }

    public static Task<List<T>> GetStatelessAsync<T>() where T : Entity.IEntity
    {
      using (var session = Mapping.GetStatelessSession())
      {
        return session.Query<T>().ToListAsync();
      }
    }

    public static Guid GetDatabaseUniqueId()
    {
      using (var context = GetEntityContext("Get database unique id"))
      {
        var id = context.Get<DatabaseConfig>().Single().UniqueId;
        Log.Add($"Database unique id = {id:D}");
        return id;
      }
    }
  }
}
