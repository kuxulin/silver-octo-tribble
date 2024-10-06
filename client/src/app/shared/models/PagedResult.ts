export default interface pagedResult<TResult> {
  items: TResult[];
  totalCount: number;
}
