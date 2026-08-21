import type { TaskStatus } from "../types/task";

export function formatStatus(status: TaskStatus): string {
  switch (status) {
    case "ToDo":
      return "To Do";
    case "InProgress":
      return "In Progress";
    case "Done":
      return "Done";
  }
}

export function formatDate(isoDate: string): string {
  return new Date(isoDate).toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}
