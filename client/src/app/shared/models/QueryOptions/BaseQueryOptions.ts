export default interface BaseQueryOptions {
  pageSize: number | undefined;
  pageIndex: number | undefined;
  sortField: string | undefined;
  sortByDescending: boolean | undefined;
}
