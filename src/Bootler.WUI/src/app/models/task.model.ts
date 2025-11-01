export interface Task {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
  assignedUserId?: number;
  createdAt: Date;
  updatedAt: Date;
}

export enum TaskStatus {
  Pending = 'Pending',
  InProgress = 'InProgress',
  Completed = 'Completed'
}

export interface TaskCreateRequest {
  title: string;
  description: string;
}

export interface TaskUpdateRequest {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
}

export interface TaskAssignRequest {
  taskId: number;
  userId: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}