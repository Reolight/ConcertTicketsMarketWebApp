﻿using Microsoft.EntityFrameworkCore;

namespace SorterByCriteria;

internal static class SorterExtensions
{
    /// <summary>
    /// This extension method sorts collection according to passed criteria. It works slower then usual
    /// sorting by OrderBy(Descending) by 25% but provides less code writing due to auto resolving of
    /// sorting criteria. 
    /// </summary>
    /// <param name="queryable">Queryable collection to sort</param>
    /// <param name="criteria">Criteria collection</param>
    /// <typeparam name="T">The type of underlying sorting collection</typeparam>
    /// <returns>IOrderedQueryable sorted by criteria</returns>
    internal static IQueryable<T> SortByCriteria<T>(
        this IQueryable<T> queryable,
        List<SortingCriterion>? criteria)
    {
        // ReSharper disable once NullableWarningSuppressionIsUsed
        IOrderedQueryable<T> ordered = null!;
        for (int index = 0; index < criteria?.Count; index++)
        {
            var criterion = criteria[index];
            if (index == 0)
                ordered = criterion.IsAsc
                    ? queryable.OrderBy(t => EF.Property<object>(t, criterion.Name))
                    : queryable.OrderByDescending(t => EF.Property<object>(t, criterion.Name));
            else
                ordered = criterion.IsAsc
                    ? ordered.ThenBy(t => EF.Property<object>(t, criterion.Name))
                    : ordered.ThenByDescending(t => EF.Property<object>(t, criterion.Name));
        }

        return ordered ?? queryable;
    }
}