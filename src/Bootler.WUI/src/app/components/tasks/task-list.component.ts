import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSortModule } from '@angular/material/sort';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSortModule
  ],
  template: `
    <div class="task-list-container">
      <div class="header-actions">
        <h2>Tasks</h2>
        <button mat-raised-button color="primary" (click)="openCreateDialog()">
          <mat-icon>add</mat-icon>
          New Task
        </button>
      </div>

      <mat-form-field>
        <mat-label>Filter</mat-label>
        <input matInput (keyup)="applyFilter($event)" placeholder="Search tasks...">
      </mat-form-field>

      <mat-table [dataSource]="dataSource" matSort>
        <ng-container matColumnDef="title">
          <mat-header-cell *matHeaderCellDef mat-sort-header>Title</mat-header-cell>
          <mat-cell *matCellDef="let task">{{task.title}}</mat-cell>
        </ng-container>

        <ng-container matColumnDef="status">
          <mat-header-cell *matHeaderCellDef mat-sort-header>Status</mat-header-cell>
          <mat-cell *matCellDef="let task">{{task.status}}</mat-cell>
        </ng-container>

        <ng-container matColumnDef="assignedUser">
          <mat-header-cell *matHeaderCellDef>Assigned To</mat-header-cell>
          <mat-cell *matCellDef="let task">{{task.assignedUser?.userName || 'Unassigned'}}</mat-cell>
        </ng-container>

        <ng-container matColumnDef="actions">
          <mat-header-cell *matHeaderCellDef>Actions</mat-header-cell>
          <mat-cell *matCellDef="let task">
            <button mat-icon-button [matMenuTriggerFor]="menu">
              <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu #menu="matMenu">
              <button mat-menu-item (click)="editTask(task)">
                <mat-icon>edit</mat-icon>
                <span>Edit</span>
              </button>
              <button mat-menu-item (click)="deleteTask(task)">
                <mat-icon>delete</mat-icon>
                <span>Delete</span>
              </button>
              <button mat-menu-item (click)="assignTask(task)" *ngIf="!task.assignedUserId">
                <mat-icon>person_add</mat-icon>
                <span>Assign</span>
              </button>
              <button mat-menu-item (click)="unassignTask(task)" *ngIf="task.assignedUserId">
                <mat-icon>person_remove</mat-icon>
                <span>Unassign</span>
              </button>
            </mat-menu>
          </mat-cell>
        </ng-container>

        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>

      <mat-paginator [pageSizeOptions]="[5, 10, 25, 100]" showFirstLastButtons></mat-paginator>
    </div>
  `,
  styles: [`
    .task-list-container {
      padding: 20px;
    }
    .header-actions {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }
    mat-form-field {
      width: 100%;
      margin-bottom: 20px;
    }
  `]
})
export class TaskListComponent implements OnInit {
  displayedColumns: string[] = ['title', 'status', 'assignedUser', 'actions'];
  dataSource: MatTableDataSource<Task>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private taskService: TaskService) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe(response => {
      this.dataSource.data = response.items;
      this.dataSource.paginator = this.paginator;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  openCreateDialog() {
    // TODO: Implement create task dialog
  }

  editTask(task: Task) {
    // TODO: Implement edit task dialog
  }

  deleteTask(task: Task) {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(task.id).subscribe(() => {
        this.loadTasks();
      });
    }
  }

  assignTask(task: Task) {
    // TODO: Implement assign task dialog
  }

  unassignTask(task: Task) {
    // TODO: Implement unassign task functionality
  }
}