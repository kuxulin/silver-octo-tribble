import Image from '../Image';

export default interface UserUpdateDTO {
  id: number;
  userName: string | null;
  firstName: string | null;
  lastName: string | null;
  phoneNumber: string | null;
  imageDto: Image;
}
