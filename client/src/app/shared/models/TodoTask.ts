import Employee from './Employee';
import AvailableTaskStatus from './enums/AvailableStatus';

export default interface TodoTask {
  id: string;
  title: string;
  text: string;
  status: AvailableTaskStatus;
  employee: Employee | null;
  projectId: string;
}
