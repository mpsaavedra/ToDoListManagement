import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { TaskService } from '../../../core/services/task.service';
import { Task } from '../../../core/models/task.model';
import { TaskFormDialogComponent } from '../components/task-form-dialog/task-form-dialog.component';
// import { TaskFormDialogComponent } from '../components/task-form-dialog/task-form-dialog.componxent';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { AuthService } from '../../../core/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  displayedColumns: string[] = ['title', 'description', 'dueDate', 'state', 'actions'];
  dataSource = new MatTableDataSource<Task>();
  totalTasks = 0;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private taskService: TaskService,
    public authService: AuthService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(pageIndex = 0, pageSize = 10): void {
    const currentUser = this.authService.currentUserValue;
    if (!currentUser || !currentUser.userName) return;

    // El filtro debe coincidir con lo que espera tu API.
    // Ejemplo: "Users.UserName==\"usuario_actual\""
    const filter = `Users.UserName=="${currentUser.userName}"`;

    this.taskService.getTasks(pageIndex, pageSize, [filter]).subscribe(response => {
      if (response.success) {
        this.dataSource.data = response.data.items;
        this.totalTasks = response.data.totalCount;
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.loadTasks(event.pageIndex, event.pageSize);
  }

  openTaskDialog(task?: Task): void {
    const dialogRef = this.dialog.open(TaskFormDialogComponent, {
      width: '500px',
      data: { task: task }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTasks(this.paginator.pageIndex, this.paginator.pageSize);
        this.snackBar.open('Tarea guardada exitosamente', 'Cerrar', { duration: 3000 });
      }
    });
  }

  deleteTask(taskId: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar esta tarea?')) {
      this.taskService.deleteTask(taskId).subscribe(() => {
        this.loadTasks(this.paginator.pageIndex, this.paginator.pageSize);
        this.snackBar.open('Tarea eliminada', 'Cerrar', { duration: 3000 });
      });
    }
  }
}
