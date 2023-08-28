using SorterByCriteria;
using SorterByCriteria.DI;
using SorterByCriteria.FilterFeature;
using SorterByCriteria.FilterFeature.Enums;
using SorterByCriteria.QueryFeature;
using SorterTests.Data;

namespace SorterTests;

public partial class QueryParser
{
    public static IEnumerable<object[]> JsonForTests = new[]
    {
        new object[]
        {
            @"{
	            ""page"": 0,
                ""count"": 20
            }"
        },
        new object[]
        {
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
            }"
        },
        new object[]
        {
            @"{
                ""filters"": { ""age"": { ""gt"": 18 }}
            }"
        },
        new object[]
        {
            @"{
                ""filters"": {""and"": [{""age"":{""gt"":18}},{""age"":{""lt"":42}} ] }
            }"
        },
        new object[]
        {
            @"{
                ""filters"": {""or"": [{""money"":{""ge"":10000}}, {""and"": [{""age"":{""gt"":18}}, {""age"":{""le"":42}} ] } ]}
            }"
        }
    };

    public static IEnumerable<object[]> FullApplyer = new[]
    {
        new[]
        {
            new Func<IQueryable<Person>, (IEnumerable<Person>, int)>(
                queryable => (queryable.Skip(0).Take(20).ToList(), (int)Math.Ceiling(queryable.Count() / 20.0)))
        },
        new[]
        {
            new Func<IQueryable<Person>, (IEnumerable<Person>, int)>(
                queryable => (queryable.OrderByDescending(person => person.Name)
                        .ThenBy(person => person.Money).Skip(15).Take(15).ToList(),
                    (int)Math.Ceiling(queryable.Count() / 15.0)))
        },
        new[]
        {
            new Func<IQueryable<Person>, (IEnumerable<Person>, int)>(
                queryable => (queryable.Where(person => person.Age > 18)
                        .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                        .ToList(),
                    (int)Math.Ceiling(queryable.Count() /
                                      (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage))
            )
        },
        new[]
        {
            new Func<IQueryable<Person>, (IEnumerable<Person>, int)>(
                queryable => (queryable.Where(person => person.Age > 18 && person.Age < 42)
                        .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                        .ToList(),
                    (int)Math.Ceiling(queryable.Count() /
                                      (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage))
            )
        },
        new[]
        {
            new Func<IQueryable<Person>, (IEnumerable<Person>, int)>(
                queryable => (queryable.Where(person => person.Money >= 10000 || person.Age > 18 && person.Age <= 42)
                        .Take(FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage)
                        .ToList(),
                    (int)Math.Ceiling(queryable.Count() /
                                      (double)FilterSorterPaginatorConfigurations.DefaultCountOfElementsOnPage))
            )
        }
    };

    public static IEnumerable<object[]> LambdaFilters = new[]
    {
        new[] { new Func<Person, bool>(person => person.Age > 18) },
        new[] { new Func<Person, bool>(person => person.Age > 18 && person.Age < 42) },
        new[] { new Func<Person, bool>(person => person.Money >= 10000 || person.Age is > 18 and <= 42) }
    };

    public static IEnumerable<object[]> AdvancedQueries = new[]
    {
        new []{ new AdvancedQuery<Person> { Count = 20, Page = 0 } },
        new []{ new AdvancedQuery<Person>
        {
            Count = 15,
            Page = 1,
            Sorting = new List<SortingCriterion>(new[]
            {
                new SortingCriterion{ Name = "age", IsAsc = false},
                new SortingCriterion{ Name = "money",IsAsc = true}
            })
        }},
        new []{new AdvancedQuery<Person>
        {
            Filters = new SimpleFilter<int>
            {
                PropertyName = "age",
                CompareExpression = new CompareExpression<int>
                {
                    Value = 18,
                    CompareType = CompareType.GreaterThen
                }
            }
        }},
        new []{new AdvancedQuery<Person>
        {
            Filters = new ComplexFilter
            {
                Conjunction = ConjunctionType.And,
                Filters = new List<FilterBase>(new []
                {
                    new SimpleFilter<int>
                    {
                        PropertyName = "age",
                        CompareExpression = new CompareExpression<int>
                        {
                            CompareType = CompareType.GreaterThen, 
                            Value = 18
                        }
                    },
                    new SimpleFilter<int>
                    {
                        PropertyName = "age",
                        CompareExpression = new CompareExpression<int>
                        {
                            CompareType = CompareType.LessThen,
                            Value = 42
                        }
                    }
                })
            }
        }},
        new[]
        {
            new AdvancedQuery<Person>
            {
                Filters = new ComplexFilter
                {
                    Conjunction = ConjunctionType.Or,
                    Filters = new List<FilterBase>(new FilterBase[]{new SimpleFilter<int>
                    {
                        PropertyName = "money",
                        CompareExpression = new CompareExpression<int>
                        {
                            CompareType = CompareType.GreaterEqual,
                            Value = 10_000
                        }
                    }, new ComplexFilter
                    {
                        Conjunction = ConjunctionType.And,
                        Filters = new List<FilterBase>(new []
                        {
                            new SimpleFilter<int>
                            {
                                PropertyName = "age",
                                CompareExpression = new CompareExpression<int>
                                {
                                    CompareType = CompareType.GreaterThen, 
                                    Value = 18
                                }
                            },
                            new SimpleFilter<int>
                            {
                                PropertyName = "age",
                                CompareExpression = new CompareExpression<int>
                                {
                                    CompareType = CompareType.LessEqual,
                                    Value = 42
                                }
                            }
                        })
                    } 
                })
            }
        }}
    };

}