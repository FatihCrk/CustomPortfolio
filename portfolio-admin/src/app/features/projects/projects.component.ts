import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule
  ],
  template: `
    <div class="projects-container">
      <mat-toolbar>
        <h1>Project Management</h1>
        <span class="spacer"></span>
        <button mat-raised-button color="primary">
          <mat-icon>add</mat-icon>
          Add Project
        </button>
      </mat-toolbar>
      
      <mat-grid-list cols="3" rowHeight="300px" gutterSize="16px">
        @for (project of projects; track project.id) {
          <mat-grid-tile>
            <mat-card class="project-card">
              <img mat-card-image [src]="project.image" [alt]="project.title">
              <mat-card-header>
                <mat-card-title>{{ project.title }}</mat-card-title>
                <mat-card-subtitle>{{ project.category }}</mat-card-subtitle>
              </mat-card-header>
              <mat-card-content>
                <p>{{ project.description }}</p>
              </mat-card-content>
              <mat-card-actions>
                <button mat-button color="primary">Edit</button>
                <button mat-button color="warn">Delete</button>
              </mat-card-actions>
            </mat-card>
          </mat-grid-tile>
        }
      </mat-grid-list>
    </div>
  `,
  styles: [`
    .projects-container {
      padding: 24px;
    }
    
    .spacer {
      flex: 1 1 auto;
    }
    
    .project-card {
      width: 100%;
      height: 100%;
      display: flex;
      flex-direction: column;
    }
    
    .project-card img {
      object-fit: cover;
      height: 150px;
    }
    
    .project-card mat-card-content {
      flex: 1;
    }
  `]
})
export class ProjectsComponent {
  projects = [
    {
      id: 1,
      title: 'E-Commerce Platform',
      category: 'Web Development',
      description: 'Full-stack e-commerce solution with payment integration',
      image: 'https://via.placeholder.com/400x200?text=E-Commerce'
    },
    {
      id: 2,
      title: 'Mobile Banking App',
      category: 'Mobile Development',
      description: 'Cross-platform mobile banking application',
      image: 'https://via.placeholder.com/400x200?text=Banking+App'
    },
    {
      id: 3,
      title: 'AI Dashboard',
      category: 'Data Science',
      description: 'Machine learning powered analytics dashboard',
      image: 'https://via.placeholder.com/400x200?text=AI+Dashboard'
    }
  ];
}
