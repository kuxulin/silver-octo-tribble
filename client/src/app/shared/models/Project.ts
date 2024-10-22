import Employee from './Employee';
import Manager from './Manager';

export default interface Project {
  id: string;
  name: string;
  description: string;
  managers: Manager[];
  employees: Employee[];
}
