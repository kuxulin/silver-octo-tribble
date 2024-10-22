import AvailableTaskStatus from '../enums/AvailableStatus';

export default interface TodoTaskCreateDTO {
  title: string;
  text: string;
  employeeId: string;
  projectId: string;
}