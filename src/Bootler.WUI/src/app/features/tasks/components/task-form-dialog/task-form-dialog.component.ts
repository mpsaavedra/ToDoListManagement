import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Task, TaskState } from '../../../../core/models/task.model';
import { TaskService } from '../../../../core/services/task.service';
import { AuthService } from '../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../../shared/material.module';

@Component({
  selector: 'app-task-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MaterialModule],
  templateUrl: './task-form-dialog.component.html',
  styleUrls: ['./task-form-dialog.component.scss'] 
})
export class TaskFormDialogComponent implements OnInit {
  taskForm!: FormGroup;
  isEditMode: boolean;
  taskStates: TaskState[] = ['Pending', 'Confirmed', 'Finished'];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private authService: AuthService,
    public dialogRef: MatDialogRef<TaskFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { task?: Task }
  ) {
    this.isEditMode = !!data.task;
  }

  ngOnInit(): void {
    this.taskForm = this.fb.group({
      title: [this.data.task?.title || '', Validators.required],
      description: [this.data.task?.description || ''],
      dueDate: [this.data.task?.dueDate ? new Date(this.data.task.dueDate) : null],
      stateType: [this.data.task?.stateType || 'Pending']
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    if (this.taskForm.invalid) {
      return;
    }

    const formValue = this.taskForm.value;
    const dueDate = formValue.dueDate ? new Date(formValue.dueDate).toISOString() : undefined;

    if (this.isEditMode && this.data.task) {
      const updateRequest = {
        id: this.data.task.id,
        ...formValue,
        dueDate: dueDate
      };
      this.taskService.updateTask(updateRequest).subscribe(() => {
        this.dialogRef.close(true);
      });
    } else {
      const currentUser = this.authService.currentUserValue;
      const createRequest = {
        ...formValue,
        dueDate: dueDate,
        userName: currentUser?.userName
      };
      this.taskService.createTask(createRequest).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }
}
