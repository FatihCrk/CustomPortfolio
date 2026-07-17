import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatIconModule
  ],
  template: `
    <div class="dashboard">
      <h1>Dashboard</h1>
      
      <mat-grid-list cols="4" rowHeight="200px" gutterSize="16px">
        <mat-grid-tile>
          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-icon users">
                <mat-icon>people</mat-icon>
              </div>
              <div class="stat-info">
                <h3>{{ stats.users }}</h3>
                <p>Total Users</p>
              </div>
            </mat-card-content>
          </mat-card>
        </mat-grid-tile>
        
        <mat-grid-tile>
          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-icon projects">
                <mat-icon>folder</mat-icon>
              </div>
              <div class="stat-info">
                <h3>{{ stats.projects }}</h3>
                <p>Total Projects</p>
              </div>
            </mat-card-content>
          </mat-card>
        </mat-grid-tile>
        
        <mat-grid-tile>
          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-icon blogs">
                <mat-icon>article</mat-icon>
              </div>
              <div class="stat-info">
                <h3>{{ stats.blogs }}</h3>
                <p>Total Blogs</p>
              </div>
            </mat-card-content>
          </mat-card>
        </mat-grid-tile>
        
        <mat-grid-tile>
          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-icon messages">
                <mat-icon>message</mat-icon>
              </div>
              <div class="stat-info">
                <h3>{{ stats.messages }}</h3>
                <p>Total Messages</p>
              </div>
            </mat-card-content>
          </mat-card>
        </mat-grid-tile>
      </mat-grid-list>
      
      <div class="welcome-section">
        <mat-card>
          <mat-card-header>
            <mat-card-title>Welcome, {{ userName }}!</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p>You are successfully logged in to Portfolio Admin Panel.</p>
            <p>Use the sidebar to navigate through different sections.</p>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard {
      padding: 24px;
    }
    
    h1 {
      margin-bottom: 24px;
      color: #333;
    }
    
    .stat-card {
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    
    .stat-card mat-card-content {
      display: flex;
      align-items: center;
      gap: 16px;
      width: 100%;
    }
    
    .stat-icon {
      width: 60px;
      height: 60px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 32px;
    }
    
    .stat-icon.users { background: #e3f2fd; color: #1976d2; }
    .stat-icon.projects { background: #e8f5e9; color: #388e3c; }
    .stat-icon.blogs { background: #fff3e0; color: #f57c00; }
    .stat-icon.messages { background: #fce4ec; color: #c2185b; }
    
    .stat-info h3 {
      margin: 0;
      font-size: 28px;
      color: #333;
    }
    
    .stat-info p {
      margin: 4px 0 0;
      color: #666;
      font-size: 14px;
    }
    
    .welcome-section {
      margin-top: 24px;
    }
  `]
})
export class DashboardComponent {
  private authService = inject(AuthService);
  
  userName = '';
  stats = {
    users: 0,
    projects: 0,
    blogs: 0,
    messages: 0
  };

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.userName = user.firstName || user.userName;
    }
    
    // Mock data - will be replaced with real API calls
    setTimeout(() => {
      this.stats = {
        users: 5,
        projects: 12,
        blogs: 8,
        messages: 23
      };
    }, 500);
  }
}
