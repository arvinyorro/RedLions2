// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="">
// Developer: Arvin Yorro
// </copyright>
// -----------------------------------------------------------------------

namespace RedLions.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRepository
    {
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IEnumerable<TEntity> GetPagedList<TEntity>(int pageIndex, int pageSize) where TEntity : class;

        IEnumerable<TEntity> GetPagedList<TEntity, TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<TEntity, TKey>> order,
            Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        TEntity GetById<TEntity>(int id) where TEntity : class;
        TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}