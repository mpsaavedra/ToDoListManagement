import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';

// Import components for routing
import { LoginComponent } from './app/components/auth/login.component';
import { TaskListComponent } from './app/components/tasks/task-list.component';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideAnimations(),
    provideRouter([
      { path: 'login', component: LoginComponent },
      { path: 'tasks', component: TaskListComponent },
      { path: '', redirectTo: '/login', pathMatch: 'full' }
    ])
  ]
}).catch(err => console.error(err));
