export default interface UserAuthDTO {
  token: string;
  userName: string;
  roles: string[];
  id: number;
  isBlocked: boolean;
}
