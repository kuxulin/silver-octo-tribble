import AvailableTaskStatus from '../enums/AvailableStatus';

export default interface TodoTaskUpdateDTO {
  id: string;
  title: string;
  text: string;
  employeeId: string;
  status: AvailableTaskStatus;
}
