export type TaskState = 'Pending' | 'Confirmed' | 'Finished';

export interface Task {
  id: number;
  title?: string;
  description?: string;
  stateType?: TaskState;
  dueDate?: string; // ISO date string
  users?: any[]; // Simplificado por brevedad
}

export interface TaskCreateRequest {
  title?: string;
  description?: string;
  dueDate?: string;
  userName?: string;
}

export interface TaskUpdateRequest {
  id: number;
  title?: string;
  description?: string;
  stateType?: TaskState;
  dueDate?: string;
}
