import { produce } from "immer"


function create_simple_filter(name, comparison, value){
    return { [name] : { [comparison] : value } }
}

function is_complex_filter(filter) {
    return filter.filters.and || filter.filters.or;
}

function get_nested_simple_filters_count(complex_filter){
    return (complex_filter.filters.and && complex_filter.filters.and.length) 
        || (complex_filter.filters.or  && complex_filter.filters.or .length);
}

export const DEFAULT_PAGE_COUNT = 20
// query: page, count, sorting, filters
export class AdvancedQueryBuilder {
    _query = {}

    getQuery() { 
        return { ...this._query };
    }

    buildQuery() {
        return JSON.stringify(this._query);
    }

    addSortingCriterion({ name, isAsc }){
        this._query.sorting.push({ name: name, isAsc: isAsc });
    }

    removeSortingCriterion(name){
        this._query = this._query.sorting.filter(sc => sc.name !== name);
    }

    changePage(page, count) {
        this.page = page? page : null;
        this.count = count || count === DEFAULT_PAGE_COUNT ? count : null;
    }

    addFilter(name, comparison, value) {
        if (!Object.keys(this._query.filters).length)
            this._query.filters = this.createSimpleFilter(name, comparison, value);
        else if (is_complex_filter(this._query)) {
            const simple_filter = this._query.filters;
            this._query.filter = { and : [ simple_filter, create_simple_filter(name, comparison, value) ]}
        } else {
            this._query.filter.and.push(create_simple_filter(name, comparison, value));
        }
    }

    removeFilter(name) {
        if (is_complex_filter(this._query.filters) &&
            get_nested_simple_filters_count(this._query.filters) > 1)
        {
            // It is supposed, that there is 'and' filter available only.
            this._query.filters.and = this._query.filters.and.filter(simple_filter => 
                !(`${name}` in simple_filter)
            );

            if (get_nested_simple_filters_count(this._query.filters) === 1){
                const simple_filter = this._query.filters.and[0];
                this._query.filters = simple_filter;
            }
        } else if (`${name}` in this._query.filters) {
            this._query.filters = null;
        }
    }
}