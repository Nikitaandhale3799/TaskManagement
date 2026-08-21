import { FilterBar } from "./components/FilterBar";
import { TaskForm } from "./components/TaskForm";
import { TaskTable } from "./components/TaskTable";
import { useTasks } from "./hooks/useTasks";

function App() {
  const {
    tasks,
    loading,
    error,
    statusFilter,
    priorityFilter,
    setStatusFilter,
    setPriorityFilter,
    refetch,
    updateTaskStatus,
    deleteTask,
  } = useTasks();

  return (
    <div className="page">
      <header className="page__header">
        <h1>Task Management</h1>
      </header>

      <TaskForm onCreated={refetch} />

      <FilterBar
        statusFilter={statusFilter}
        priorityFilter={priorityFilter}
        onStatusChange={setStatusFilter}
        onPriorityChange={setPriorityFilter}
      />

      <main className="page__content">
        {loading && <p className="state-message">Loading tasks…</p>}
        {!loading && error && (
          <div className="state-message state-message--error">
            <p>{error}</p>
            <button type="button" className="state-message__retry" onClick={refetch}>
              Retry
            </button>
          </div>
        )}
        {!loading && !error && tasks.length === 0 && (
          <p className="state-message">No tasks match the current filters.</p>
        )}
        {!loading && !error && tasks.length > 0 && (
          <TaskTable tasks={tasks} onStatusChange={updateTaskStatus} onDelete={deleteTask} />
        )}
      </main>
    </div>
  );
}

export default App;
