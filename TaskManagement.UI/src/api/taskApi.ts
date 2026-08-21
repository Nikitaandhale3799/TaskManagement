import type {
  Task,
  TaskStatus,
  TaskPriority,
  CreateTaskRequest,
  UpdateTaskRequest,
} from "../types/task";

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5267/api";

export class ApiError extends Error {
  status: number;

  constructor(status: number, message: string) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}

interface ProblemDetailsBody {
  title?: string;
  errors?: Record<string, string[]>;
}

function extractErrorMessage(body: ProblemDetailsBody | null): string | null {
  if (!body) return null;

  if (body.errors) {
    const messages = Object.values(body.errors).flat();
    if (messages.length > 0) return messages.join(" ");
  }

  return body.title ?? null;
}

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const body: ProblemDetailsBody | null = await response.json().catch(() => null);
    const message = extractErrorMessage(body) ?? `Request failed with status ${response.status}`;
    throw new ApiError(response.status, message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

export interface GetTasksParams {
  status?: TaskStatus;
  priority?: TaskPriority;
  sortBy?: string;
  descending?: boolean;
}

export async function getTasks(params: GetTasksParams = {}): Promise<Task[]> {
  const query = new URLSearchParams();
  if (params.status) query.set("status", params.status);
  if (params.priority) query.set("priority", params.priority);
  if (params.sortBy) query.set("sortBy", params.sortBy);
  if (params.descending) query.set("descending", String(params.descending));

  const queryString = query.toString();
  const response = await fetch(`${BASE_URL}/tasks${queryString ? `?${queryString}` : ""}`);
  return handleResponse<Task[]>(response);
}

export async function createTask(dto: CreateTaskRequest): Promise<Task> {
  const response = await fetch(`${BASE_URL}/tasks`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
  return handleResponse<Task>(response);
}

export async function updateTask(id: number, dto: UpdateTaskRequest): Promise<Task> {
  const response = await fetch(`${BASE_URL}/tasks/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
  return handleResponse<Task>(response);
}

export async function deleteTask(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/tasks/${id}`, {
    method: "DELETE",
  });
  return handleResponse<void>(response);
}
