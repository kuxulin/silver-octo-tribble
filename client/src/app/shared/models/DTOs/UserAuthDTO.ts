import AvailableUserRole from '../enums/AvailableUserRole';

export default interface UserAuthDTO {
  token: string;
  userName: string;
  roles: AvailableUserRole[];
  id: number;
  isBlocked: boolean;
  employeeId: string | undefined;
  managerId: string | undefined;
}
