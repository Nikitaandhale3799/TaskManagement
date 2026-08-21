import { useCallback, useEffect, useState } from "react";
import { ApiError, deleteTask as deleteTaskRequest, getTasks, updateTask } from "../api/taskApi";
import type { Task, TaskPriority, TaskStatus } from "../types/task";

export function useTasks() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [statusFilter, setStatusFilter] = useState<TaskStatus | "">("");
  const [priorityFilter, setPriorityFilter] = useState<TaskPriority | "">("");

  const fetchTasks = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getTasks({
        status: statusFilter || undefined,
        priority: priorityFilter || undefined,
        descending: true,
      });
      setTasks(data);
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to load tasks. Please try again.");
    } finally {
      setLoading(false);
    }
  }, [statusFilter, priorityFilter]);

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  const updateTaskStatus = useCallback(
    async (id: number, newStatus: TaskStatus): Promise<string | null> => {
      const target = tasks.find((t) => t.id === id);
      if (!target) return "Task not found.";

      const previousStatus = target.status;

      setTasks((prev) => prev.map((t) => (t.id === id ? { ...t, status: newStatus } : t)));

      try {
        const updated = await updateTask(id, {
          title: target.title,
          description: target.description,
          status: newStatus,
          priority: target.priority,
          assignedTo: target.assignedTo,
        });
        setTasks((prev) => prev.map((t) => (t.id === id ? updated : t)));
        return null;
      } catch (err) {
        setTasks((prev) => prev.map((t) => (t.id === id ? { ...t, status: previousStatus } : t)));
        return err instanceof ApiError ? err.message : "Failed to update status. Please try again.";
      }
    },
    [tasks]
  );

  const deleteTask = useCallback(async (id: number): Promise<string | null> => {
    try {
      await deleteTaskRequest(id);
      setTasks((prev) => prev.filter((t) => t.id !== id));
      return null;
    } catch (err) {
      return err instanceof ApiError ? err.message : "Failed to delete task. Please try again.";
    }
  }, []);

  return {
    tasks,
    loading,
    error,
    statusFilter,
    priorityFilter,
    setStatusFilter,
    setPriorityFilter,
    refetch: fetchTasks,
    updateTaskStatus,
    deleteTask,
  };
}
