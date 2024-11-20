import AvailableUserRole from './enums/AvailableUserRole';
import Image from './Image';

export default interface User {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  phoneNumber: string;
  roleIds: AvailableUserRole[];
  creationDate: Date;
  isBlocked: boolean;
  imageId: string;
  image: Image;
  managerId: string | undefined;
  employeeId: string | undefined;
}
