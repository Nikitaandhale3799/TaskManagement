import { useState } from "react";
import type { Task } from "../types/task";

interface DeleteButtonProps {
  task: Task;
  onDelete: (id: number) => Promise<string | null>;
}

export function DeleteButton({ task, onDelete }: DeleteButtonProps) {
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleClick() {
    const confirmed = window.confirm(`Delete "${task.title}"? This cannot be undone from the UI.`);
    if (!confirmed) return;

    setDeleting(true);
    setError(null);

    const failureMessage = await onDelete(task.id);

    setDeleting(false);
    if (failureMessage) setError(failureMessage);
  }

  return (
    <div className="delete-button">
      <button type="button" className="delete-button__control" onClick={handleClick} disabled={deleting}>
        {deleting ? "Deleting…" : "Delete"}
      </button>
      {error && <span className="delete-button__error">{error}</span>}
    </div>
  );
}
