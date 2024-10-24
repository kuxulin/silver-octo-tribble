import Image from '../Image';

export default interface LoginRegisterDTO {
  userName: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  password: string;
  image: Image;
}
