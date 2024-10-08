import AvailableUserRole from '../enums/AvailableUserRole';
import BaseQueryOptions from './BaseQueryOptions';

export default interface UserQueryOptions extends BaseQueryOptions {
  partialUserName: string | undefined;
  filterRoles: AvailableUserRole[] | undefined;
  isBlocked: boolean | undefined;
  startDate: Date | undefined;
  endDate: Date | undefined;
}
