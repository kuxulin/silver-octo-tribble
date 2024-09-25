export default interface AuthDTO {
  token: string;
  name: string;
  roles: string[] | undefined;
}
