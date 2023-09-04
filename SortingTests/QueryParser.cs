using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SorterByCriteria;
using SorterByCriteria.DI;
using SorterByCriteria.QueryFeature;
using SorterTests.Data;
using Xunit.Abstractions;

namespace SorterTests;

public partial class QueryParser
{
    private readonly ITestOutputHelper _testOutputHelper;

    private static readonly ServiceProvider _provider;

    public static IEnumerable<object[]> JsonToQuery
    {
        get
        {
            using var jsonEnum = JsonForTests.GetEnumerator();
            using var advqEnum = AdvancedQueries.GetEnumerator();

            while (jsonEnum.MoveNext() && advqEnum.MoveNext()) {
                yield return new object[] { jsonEnum.Current.First(), advqEnum.Current.First() };
            } 
        }
    }

    public static IEnumerable<object[]> AdvQueriesVsDefinedLambdas
    {
        get
        {
            using var jsonEnum = JsonForTests.GetEnumerator();
            using var defEnum = FullApplyer.GetEnumerator();

            while (jsonEnum.MoveNext() && defEnum.MoveNext()) {
                yield return new object[] { jsonEnum.Current.First(), defEnum.Current.First() };
            } 
        }
    }
    
    public static IEnumerable<object[]> AdvancedQueriesWithFilters
    {
        get
        {
            using var advqEnum = AdvancedQueries.GetEnumerator();
            while (advqEnum.MoveNext())
            {
                var advq = advqEnum.Current.First() as AdvancedQuery<Person>;
                if (advq is not { Filters: null })
                    yield return new object[] { advq };
            }
        }
    }

    public static IEnumerable<object[]> AdvancedQueriesVsReadyLambdas
    {
        get
        {
            using var advqEnum = AdvancedQueries.GetEnumerator();
            using var compLambdas = LambdaFilters.GetEnumerator();
            bool lambdasLeft = true;
            while (advqEnum.MoveNext() && lambdasLeft)
            {
                if (advqEnum.Current.First() is not AdvancedQuery<Person> { Filters: { } } adv) continue;
                lambdasLeft = compLambdas.MoveNext();
                yield return new[] { adv, compLambdas.Current.First() };
            }
        }
    }
    
    static QueryParser()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<Context>();
        serviceCollection.AddLogging();
        serviceCollection.AddFiltersSortersPaginatorForDeveloper<Context>();
        _provider = serviceCollection.BuildServiceProvider();
        var context = _provider.GetRequiredService<Context>();
        Context.SeedData(context);
    }

    public QueryParser(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory, MemberData(nameof(JsonToQuery))]
    public void ParseJsonTest(string json, AdvancedQuery<Person> expected)
    {
        var fsp = _provider.GetRequiredService<IFspDeveloper>();
        var actual = fsp.ParseQuery<Person>(json);
        _testOutputHelper.WriteLine(json);
        _testOutputHelper.WriteLine("actual: \n" + actual.LogObject());
        _testOutputHelper.WriteLine("expect: \n" + actual.LogObject());
        Assert.Equivalent(expected, actual);
    }

    [Theory, MemberData(nameof(AdvancedQueriesWithFilters))]
    public void BuildQueries(AdvancedQuery<Person> query)
    {
        var fsp = _provider.GetRequiredService<IFspDeveloper>();
        try
        {
            var built = fsp.BuildQuery(query);
            Assert.NotNull(built);
        }
        catch (Exception e)
        {
            Assert.Fail($"Exception: {e.Message}\n{e.StackTrace}");
        }
    }

    [Theory, MemberData(nameof(AdvancedQueriesVsReadyLambdas))]
    public void FilterTest(AdvancedQuery<Person> query, Func<Person, bool> lambda)
    {
        var fsp = _provider.GetRequiredService<IFspDeveloper>();
        var context = _provider.GetRequiredService<Context>();
        var queryBuilt = fsp.BuildQuery(query);
        var expected = context.Persons.Where(lambda).ToList();
        var actual = fsp.ApplyFilters(context.Persons, queryBuilt).ToList();
        Assert.Equivalent(expected, actual);
    }

    [Theory, MemberData(nameof(AdvQueriesVsDefinedLambdas))]
    public void FullTestOfAdvancedQueries(string jsonQuery,
        Func<IQueryable<Person>, (IEnumerable<Person>, int)> expectedResultAfterExecution)
    {
        var (fsp, context) = (_provider.GetRequiredService<IFspDeveloper>(), _provider.GetRequiredService<Context>());
        var advQ = fsp.ParseQuery<Person>(jsonQuery);
        var builtQ = fsp.BuildQuery(advQ);

        var filtered = fsp.ApplyFilters(context.Persons, builtQ);
        var sorted = fsp.ApplySorting(filtered);
        var (actual, _) = fsp.ApplyPagination(sorted);
                
        var (expectedList, _) = expectedResultAfterExecution.Invoke(context.Persons);
        Assert.Equivalent(expectedList, actual.ToList());
    }
}