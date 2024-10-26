import AvailableAction from './enums/AvailableAction';
import TodoTask from './TodoTask';
import User from './User';

export default interface Change {
  creator: User;
  task: TodoTask | undefined;
  actionType: AvailableAction;
  creationDate: Date;
  taskTitle: string | undefined;
  projectId: string;
}
