import AvailableUserRole from './enums/AvailableUserRole';
import Image from './Image';

export default interface User {
  id: number;
  firstName: string | undefined;
  lastName: string | undefined;
  userName: string | undefined;
  phoneNumber: string | undefined;
  roleIds: AvailableUserRole[] | undefined;
  creationDate: Date;
  isBlocked: boolean;
  image: Image;
}
