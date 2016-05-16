﻿
using System;
using System.Collections.Generic;

namespace DataLayer
{
     //   public interface IDBContext<T> : IDisposable where T : class
    public interface IDBContext
    {
        List<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();

        T FindOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();

        List<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, int page, int pageSize) where T : class, new();

        void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();

        void Add<T>(T item) where T : class, new();

        void AddMany<T>(IEnumerable<T> items) where T : class, new();

        void Update<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, T item) where T : class, new();

        //void UpdateMany<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, BsonDocument bsonUpdateDoc) where T : class, new();

        List<T> All<T>() where T : class, new();

        long Count<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();
    }
}
