import AvailableUserRole from './enums/AvailableUserRole';

export default interface User {
  id: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  userName: string | undefined;
  phoneNumber: string | undefined;
  roles: AvailableUserRole[] | undefined;
  creationDate: Date;
  isBlocked: boolean;
}
