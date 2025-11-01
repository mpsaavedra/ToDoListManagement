import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, TaskCreateRequest, TaskUpdateRequest, TaskAssignRequest, PaginatedResponse } from '../models/task.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = `${environment.apiUrl}/api/Task`;

  constructor(private http: HttpClient) { }

  createTask(request: TaskCreateRequest): Observable<Task> {
    return this.http.post<Task>(`${this.apiUrl}/Create`, request);
  }

  updateTask(request: TaskUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Update`, request);
  }

  deleteTask(taskId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete`, { body: { taskId } });
  }

  assignTask(request: TaskAssignRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/AssignTask`, request);
  }

  unassignTask(request: TaskAssignRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/UnassignTask`, request);
  }

  getTasks(page: number = 1, pageSize: number = 10): Observable<PaginatedResponse<Task>> {
    return this.http.get<PaginatedResponse<Task>>(`${this.apiUrl}/GetTasks`, {
      params: { page, pageSize }
    });
  }

  findTasks(searchTerm: string, page: number = 1, pageSize: number = 10): Observable<PaginatedResponse<Task>> {
    return this.http.get<PaginatedResponse<Task>>(`${this.apiUrl}/FindTasks`, {
      params: { searchTerm, page, pageSize }
    });
  }
}