export default interface ProjectUpdateDTO {
  id: string | undefined;
  name: string | undefined;
  employeeIds: string[] | undefined;
  managerIds: string[] | undefined;
}
