import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { BaseResponse, PaginatedList } from '../models/api.response.model';
import { Task, TaskCreateRequest, TaskUpdateRequest } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = `${environment.apiUrl}/Task`;

  constructor(private http: HttpClient) { }

  getTasks(pageIndex: number, pageSize: number, filters: string[] = []): Observable<BaseResponse<PaginatedList<Task>>> {
    let params = new HttpParams()
      .set('PageIndex', pageIndex.toString())
      .set('PageSize', pageSize.toString());
    
    filters.forEach(filter => {
      params = params.append('Filters', filter);
    });

    return this.http.get<BaseResponse<PaginatedList<Task>>>(`${this.apiUrl}/FindTasks`, { params });
  }

  createTask(task: TaskCreateRequest): Observable<BaseResponse<{ id: number }>> {
    return this.http.post<BaseResponse<{ id: number }>>(`${this.apiUrl}/Create`, task);
  }

  updateTask(task: TaskUpdateRequest): Observable<BaseResponse<any>> {
    return this.http.put<BaseResponse<any>>(`${this.apiUrl}/Update`, task);
  }

  deleteTask(taskId: number): Observable<BaseResponse<any>> {
    const options = {
      body: {
        taskId: taskId,
        softDelete: true // O false si quieres borrado físico
      }
    };
    return this.http.delete<BaseResponse<any>>(`${this.apiUrl}/Delete`, options);
  }
}
