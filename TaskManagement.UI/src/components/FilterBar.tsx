import type { TaskPriority, TaskStatus } from "../types/task";
import { formatStatus } from "../utils/format";

interface FilterBarProps {
  statusFilter: TaskStatus | "";
  priorityFilter: TaskPriority | "";
  onStatusChange: (value: TaskStatus | "") => void;
  onPriorityChange: (value: TaskPriority | "") => void;
}

const STATUS_OPTIONS: TaskStatus[] = ["ToDo", "InProgress", "Done"];
const PRIORITY_OPTIONS: TaskPriority[] = ["Low", "Medium", "High", "Critical"];

export function FilterBar({
  statusFilter,
  priorityFilter,
  onStatusChange,
  onPriorityChange,
}: FilterBarProps) {
  return (
    <div className="filter-bar">
      <label className="filter-bar__field">
        <span>Status</span>
        <select
          value={statusFilter}
          onChange={(e) => onStatusChange(e.target.value as TaskStatus | "")}
        >
          <option value="">All</option>
          {STATUS_OPTIONS.map((status) => (
            <option key={status} value={status}>
              {formatStatus(status)}
            </option>
          ))}
        </select>
      </label>

      <label className="filter-bar__field">
        <span>Priority</span>
        <select
          value={priorityFilter}
          onChange={(e) => onPriorityChange(e.target.value as TaskPriority | "")}
        >
          <option value="">All</option>
          {PRIORITY_OPTIONS.map((priority) => (
            <option key={priority} value={priority}>
              {priority}
            </option>
          ))}
        </select>
      </label>
    </div>
  );
}
