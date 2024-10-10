export default interface PagedResult<TResult> {
  items: TResult[];
  totalCount: number;
}
