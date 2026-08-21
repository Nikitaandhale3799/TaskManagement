export type TaskStatus = "ToDo" | "InProgress" | "Done";
export type TaskPriority = "Low" | "Medium" | "High" | "Critical";

export interface Task {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  priority: TaskPriority;
  assignedTo: string | null;
  createdDate: string;
  modifiedDate: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  status?: TaskStatus;
  priority?: TaskPriority;
  assignedTo?: string | null;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string | null;
  status: TaskStatus;
  priority: TaskPriority;
  assignedTo?: string | null;
}
