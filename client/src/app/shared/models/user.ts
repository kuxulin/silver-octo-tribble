export default interface User {
  id: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  username: string | undefined;
  phoneNumber: string | undefined;
  roles: string[] | undefined;
  creationDate: Date;
  isBlocked: boolean;
}
