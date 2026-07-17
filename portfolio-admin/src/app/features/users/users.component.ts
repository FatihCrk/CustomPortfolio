import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule
  ],
  template: `
    <div class="users-container">
      <mat-toolbar>
        <h1>User Management</h1>
        <span class="spacer"></span>
        <button mat-raised-button color="primary">
          <mat-icon>add</mat-icon>
          Add User
        </button>
      </mat-toolbar>
      
      <mat-card class="table-card">
        <table mat-table [dataSource]="users">
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef>ID</th>
            <td mat-cell *matCellDef="let user">{{ user.id }}</td>
          </ng-container>
          
          <ng-container matColumnDef="userName">
            <th mat-header-cell *matHeaderCellDef>Username</th>
            <td mat-cell *matCellDef="let user">{{ user.userName }}</td>
          </ng-container>
          
          <ng-container matColumnDef="email">
            <th mat-header-cell *matHeaderCellDef>Email</th>
            <td mat-cell *matCellDef="let user">{{ user.email }}</td>
          </ng-container>
          
          <ng-container matColumnDef="role">
            <th mat-header-cell *matHeaderCellDef>Role</th>
            <td mat-cell *matCellDef="let user">{{ user.role }}</td>
          </ng-container>
          
          <ng-container matColumnDef="isActive">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let user">
              <mat-chip [color]="user.isActive ? 'primary' : 'warn'">
                {{ user.isActive ? 'Active' : 'Inactive' }}
              </mat-chip>
            </td>
          </ng-container>
          
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let user">
              <button mat-icon-button color="primary">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn">
                <mat-icon>delete</mat-icon>
              </button>
            </td>
          </ng-container>
          
          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </mat-card>
    </div>
  `,
  styles: [`
    .users-container {
      padding: 24px;
    }
    
    .spacer {
      flex: 1 1 auto;
    }
    
    .table-card {
      margin-top: 16px;
    }
    
    table {
      width: 100%;
    }
    
    th {
      font-weight: 500;
    }
  `]
})
export class UsersComponent {
  displayedColumns: string[] = ['id', 'userName', 'email', 'role', 'isActive', 'actions'];
  
  users = [
    { id: 1, userName: 'admin', email: 'admin@portfolio.com', role: 'Super Admin', isActive: true },
    { id: 2, userName: 'editor', email: 'editor@portfolio.com', role: 'Editor', isActive: true },
    { id: 3, userName: 'viewer', email: 'viewer@portfolio.com', role: 'Viewer', isActive: false }
  ];
}
