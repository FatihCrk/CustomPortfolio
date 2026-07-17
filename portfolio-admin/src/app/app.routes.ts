import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { 
    path: 'auth', 
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES) 
  },
  { 
    path: 'dashboard', 
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  { 
    path: 'users', 
    loadComponent: () => import('./features/users/users.component').then(m => m.UsersComponent),
    canActivate: [authGuard]
  },
  { 
    path: 'projects', 
    loadComponent: () => import('./features/projects/projects.component').then(m => m.ProjectsComponent),
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'dashboard' }
];
