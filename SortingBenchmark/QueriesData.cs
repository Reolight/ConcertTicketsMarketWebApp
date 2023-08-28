using SorterByCriteria.DI;
using SorterTests.Data;

namespace SortingBenchmark;

public static class QueriesData
{
    public static readonly string PaginatorOnlyJson =

        @"{
	            ""page"": 0,
                ""count"": 20
            }";

    public static readonly string PaginationWithDoubleSortingJson =
        @"{
	            ""page"": 1,
                ""count"": 15,
                ""sorting"": [{
                    ""name"": ""age"",
                    ""isAsc"": false
                },
                {
                    ""name"": ""money"",
                    ""isAsc"": true
                }]
            }";

    public static readonly string SimpleFilterWithAgeJson =
        @"{
                ""filters"": { ""age"": { ""gt"": 18 }}
            }";

    public static readonly string ComplexFilterWithAgeRangeJson =
        @"{
                ""filters"": {""and"": [{""age"":{""gt"":18}},{""age"":{""lt"":42}} ] }
            }";

    public static readonly string ComplexFilterWithMoneyRestrictionsAndAgeRangeJson =
        @"{
                ""filters"": {""or"": [{""money"":{""ge"":10000}}, {""and"": [{""age"":{""gt"":18}}, {""age"":{""le"":42}} ] } ]}
            }";

    public static Func<IQueryable<Person>, (IEnumerable<Person>, int)> PaginatorOnlyFunc =
        queryable => (queryable.Skip(0).Take(20).ToList(), (int)Math.Ceiling(queryable.Count() / 20.0));

    public static Func<IQueryable<Person>, (IEnumerable<Person>, int)> PaginationWithDoubleSortingFunc =
        queryable => (queryable.OrderByDescending(person => person.Name)
                .ThenBy(person => person.Money).Skip(15).Take(15).ToList(),
            (int)Math.Ceiling(queryable.Count() / 15.0));

    public static Func<IQueryable<Person>, (IEnumerable<Person>, int)> SimpleFilterWithAgeFunc =
        queryable => (queryable.Where(person => person.Age > 18)
                .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                .ToList(),
            (int)Math.Ceiling(queryable.Count() /
                              (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage));

    public static Func<IQueryable<Person>, (IEnumerable<Person>, int)> ComplexFilterWithAgeRangeFunc =
        queryable => (queryable.Where(person => person.Age > 18 && person.Age < 42)
                .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                .ToList(),
            (int)Math.Ceiling(queryable.Count() /
                              (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage));

    public static Func<IQueryable<Person>, (IEnumerable<Person>, int)>
        ComplexFilterWithMoneyRestrictionsAndAgeRangeFunc =
            queryable => (queryable.Where(person => person.Money >= 10000 || person.Age > 18 && person.Age <= 42)
                    .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                    .ToList(),
                (int)Math.Ceiling(queryable.Count() /
                                  (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage));
}