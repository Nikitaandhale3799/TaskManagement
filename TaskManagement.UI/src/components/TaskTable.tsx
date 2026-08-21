import type { Task, TaskStatus } from "../types/task";
import { formatDate } from "../utils/format";
import { DeleteButton } from "./DeleteButton";
import { PriorityBadge } from "./PriorityBadge";
import { StatusSelect } from "./StatusSelect";

interface TaskTableProps {
  tasks: Task[];
  onStatusChange: (id: number, status: TaskStatus) => Promise<string | null>;
  onDelete: (id: number) => Promise<string | null>;
}

export function TaskTable({ tasks, onStatusChange, onDelete }: TaskTableProps) {
  return (
    <div className="task-table-wrapper">
      <table className="task-table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Status</th>
            <th>Priority</th>
            <th>Assigned To</th>
            <th>Created Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {tasks.map((task) => (
            <tr key={task.id}>
              <td className="task-table__title">{task.title}</td>
              <td className="task-table__description">{task.description ?? "—"}</td>
              <td>
                <StatusSelect task={task} onStatusChange={onStatusChange} />
              </td>
              <td>
                <PriorityBadge priority={task.priority} />
              </td>
              <td>{task.assignedTo ?? "Unassigned"}</td>
              <td>{formatDate(task.createdDate)}</td>
              <td>
                <DeleteButton task={task} onDelete={onDelete} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
