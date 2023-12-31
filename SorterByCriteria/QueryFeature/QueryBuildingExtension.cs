﻿using System.Linq.Expressions;

namespace SorterByCriteria.QueryFeature;

internal static class QueryBuildingExtension
{
    internal static AdvancedQueryBuilt<TObject> BuildQuery<TObject>(this AdvancedQuery<TObject> query)
    {
        var parameter = Expression.Parameter(typeof(TObject), "obj");
        return new AdvancedQueryBuilt<TObject>
        {
            Count = query.Count,
            Page = query.Page,
            Sorting = query.Sorting,
            Filters = query.Filters is not null
                ? Expression.Lambda<Func<TObject, bool>>(query.Filters.BuildFilter<TObject>(parameter), parameter)
                : null
        };
    }
}