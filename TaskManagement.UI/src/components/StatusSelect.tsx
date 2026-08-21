import { useState, type ChangeEvent } from "react";
import type { Task, TaskStatus } from "../types/task";
import { formatStatus } from "../utils/format";

interface StatusSelectProps {
  task: Task;
  onStatusChange: (id: number, status: TaskStatus) => Promise<string | null>;
}

const STATUS_OPTIONS: TaskStatus[] = ["ToDo", "InProgress", "Done"];

export function StatusSelect({ task, onStatusChange }: StatusSelectProps) {
  const [updating, setUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleChange(e: ChangeEvent<HTMLSelectElement>) {
    const newStatus = e.target.value as TaskStatus;
    setUpdating(true);
    setError(null);

    const failureMessage = await onStatusChange(task.id, newStatus);

    setUpdating(false);
    if (failureMessage) setError(failureMessage);
  }

  return (
    <div className="status-select">
      <select
        className={`status-select__control status-select__control--${task.status.toLowerCase()}`}
        value={task.status}
        onChange={handleChange}
        disabled={updating}
      >
        {STATUS_OPTIONS.map((status) => (
          <option key={status} value={status}>
            {formatStatus(status)}
          </option>
        ))}
      </select>
      {error && <span className="status-select__error">{error}</span>}
    </div>
  );
}
