import type { TaskPriority } from "../types/task";

interface PriorityBadgeProps {
  priority: TaskPriority;
}

export function PriorityBadge({ priority }: PriorityBadgeProps) {
  return (
    <span className={`priority-badge priority-badge--${priority.toLowerCase()}`}>
      {priority}
    </span>
  );
}
