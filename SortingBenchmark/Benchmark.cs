using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SorterByCriteria;
using SorterByCriteria.DI;
using SorterTests.Data;

namespace SortingBenchmark;

[MemoryDiagnoser]
public class Benchmark
{
    public enum Categories
    {
        SimplePaginator,
        PaginatorWithSorting,
        SimpleFilterGtAge,
        ComplexFilterAgeRange,
        ComplexFilterMoneyOrAgeRange
    }
    
    private static readonly ServiceProvider Provider;

    static Benchmark()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextPool<Context>(builder => builder.UseInMemoryDatabase("memory"));
        serviceCollection.AddLogging();
        serviceCollection.AddFiltersSortersPaginatorForDeveloper<Context>();
        Provider = serviceCollection.BuildServiceProvider();
        var context = Provider.GetRequiredService<Context>();
        Context.SeedData(context);
    }

    private Context _context;
    private IFspDeveloper _fsp;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _context = Provider.GetRequiredService<Context>();
        _fsp = Provider.GetRequiredService<IFspDeveloper>();
    }
    
    [Benchmark]
    public void FuncSimplePaginator()
    {
        var (data, pages) = QueriesData.PaginatorOnlyFunc(_context.Persons);
        var count = data.Count();
    }

    [Benchmark]
    public void JsonSimplePaginator()
    {
        var (data, pages) = _fsp.GetFilteredSortedPaginatedData(_context.Persons, QueriesData.PaginatorOnlyJson);
        var count = data.Count();
    }

    [Benchmark]
    public void FuncPaginatorWithSorting()
    {
        var (data, pages) = QueriesData.PaginationWithDoubleSortingFunc(_context.Persons);
        var count = data.Count();
    }

    [Benchmark]
    public void JsonPaginatorWithSorting()
    {
        var (data, pages) = _fsp.GetFilteredSortedPaginatedData(_context.Persons, QueriesData.PaginationWithDoubleSortingJson);
        var count = data.Count();
    }

    [Benchmark]
    public void FuncSimpleFilterGtAge()
    {
        var (data, pages) = QueriesData.SimpleFilterWithAgeFunc(_context.Persons);
        var count = data.Count();
    }

    [Benchmark]
    public void JsonSimpleFilterGtAge()
    {
        var (data, pages) = _fsp.GetFilteredSortedPaginatedData(_context.Persons, QueriesData.SimpleFilterWithAgeJson);
        var count = data.Count();
    }

    [Benchmark]
    public void FuncComplexFilterAgeRange()
    {
        var (data, pages) = QueriesData.ComplexFilterWithAgeRangeFunc(_context.Persons);
        var count = data.Count();
    }

    [Benchmark]
    public void JsonComplexFilterAgeRange()
    {
        var (data, pages) = _fsp.GetFilteredSortedPaginatedData(_context.Persons, QueriesData.ComplexFilterWithAgeRangeJson);
        var count = data.Count();
    }

    [Benchmark]
    public void FuncComplexFilterMoneyOrAgeRange()
    {
        var (data, pages) = QueriesData.ComplexFilterWithMoneyRestrictionsAndAgeRangeFunc(_context.Persons);
        var count = data.Count();
    }

    [Benchmark]
    public void JsonComplexFilterMoneyOrAgeRange()
    {
        var (data, pages) = _fsp.GetFilteredSortedPaginatedData(_context.Persons,
            QueriesData.ComplexFilterWithMoneyRestrictionsAndAgeRangeJson);
        var count = data.Count();
    }
}