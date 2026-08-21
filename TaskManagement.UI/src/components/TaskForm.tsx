import { useState, type FormEvent } from "react";
import { ApiError, createTask } from "../api/taskApi";
import type { TaskPriority, TaskStatus } from "../types/task";
import { formatStatus } from "../utils/format";

interface TaskFormProps {
  onCreated: () => void;
}

const STATUS_OPTIONS: TaskStatus[] = ["ToDo", "InProgress", "Done"];
const PRIORITY_OPTIONS: TaskPriority[] = ["Low", "Medium", "High", "Critical"];

export function TaskForm({ onCreated }: TaskFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [status, setStatus] = useState<TaskStatus>("ToDo");
  const [priority, setPriority] = useState<TaskPriority>("Low");
  const [assignedTo, setAssignedTo] = useState("");

  const [titleError, setTitleError] = useState<string | null>(null);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  function validate(): boolean {
    const trimmed = title.trim();
    if (!trimmed) {
      setTitleError("Title is required.");
      return false;
    }
    if (trimmed.length > 200) {
      setTitleError("Title must be 200 characters or fewer.");
      return false;
    }
    setTitleError(null);
    return true;
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setSubmitError(null);

    if (!validate()) return;

    setSubmitting(true);
    try {
      await createTask({
        title: title.trim(),
        description: description.trim() || null,
        status,
        priority,
        assignedTo: assignedTo.trim() || null,
      });

      setTitle("");
      setDescription("");
      setStatus("ToDo");
      setPriority("Low");
      setAssignedTo("");

      onCreated();
    } catch (err) {
      setSubmitError(
        err instanceof ApiError ? err.message : "Failed to create task. Please try again."
      );
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="task-form" onSubmit={handleSubmit} noValidate>
      <h2 className="task-form__heading">New Task</h2>

      <div className="task-form__row">
        <label className="task-form__field task-form__field--wide">
          <span>Title</span>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="e.g. Fix login page validation bug"
            aria-invalid={titleError ? true : undefined}
          />
          {titleError && <span className="task-form__error">{titleError}</span>}
        </label>

        <label className="task-form__field">
          <span>Status</span>
          <select value={status} onChange={(e) => setStatus(e.target.value as TaskStatus)}>
            {STATUS_OPTIONS.map((s) => (
              <option key={s} value={s}>
                {formatStatus(s)}
              </option>
            ))}
          </select>
        </label>

        <label className="task-form__field">
          <span>Priority</span>
          <select value={priority} onChange={(e) => setPriority(e.target.value as TaskPriority)}>
            {PRIORITY_OPTIONS.map((p) => (
              <option key={p} value={p}>
                {p}
              </option>
            ))}
          </select>
        </label>
      </div>

      <div className="task-form__row">
        <label className="task-form__field task-form__field--wide">
          <span>Description</span>
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} rows={2} />
        </label>

        <label className="task-form__field">
          <span>Assigned To</span>
          <input type="text" value={assignedTo} onChange={(e) => setAssignedTo(e.target.value)} />
        </label>
      </div>

      {submitError && <p className="task-form__submit-error">{submitError}</p>}

      <button type="submit" className="task-form__submit" disabled={submitting}>
        {submitting ? "Creating…" : "Create Task"}
      </button>
    </form>
  );
}
