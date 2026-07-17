import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../core/models/user.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header>
          <mat-card-title class="text-center">Portfolio Admin</mat-card-title>
          <mat-card-subtitle class="text-center">Sign in to your account</mat-card-subtitle>
        </mat-card-header>
        
        <mat-card-content>
          <form #loginForm="ngForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Username</mat-label>
              <input matInput name="userName" [(ngModel)]="username" required autocomplete="username">
            </mat-form-field>
            
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Password</mat-label>
              <input matInput name="password" type="password" [(ngModel)]="password" required autocomplete="current-password">
            </mat-form-field>
            
            <div class="flex justify-between align-center mb-2">
              <label>
                <input type="checkbox" name="rememberMe" [(ngModel)]="rememberMe">
                Remember me
              </label>
            </div>
            
            @if (errorMessage) {
              <div class="error-message">{{ errorMessage }}</div>
            }
            
            <button mat-raised-button color="primary" type="submit" class="full-width" [disabled]="isLoading">
              @if (isLoading) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                Sign In
              }
            </button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    }
    
    .login-card {
      width: 100%;
      max-width: 400px;
      padding: 20px;
    }
    
    .text-center {
      text-align: center;
    }
    
    .full-width {
      width: 100%;
    }
    
    .mb-2 {
      margin-bottom: 16px;
    }
    
    .error-message {
      color: #f44336;
      margin-bottom: 16px;
      padding: 8px;
      background: #ffebee;
      border-radius: 4px;
    }
    
    button {
      margin-top: 16px;
    }
  `]
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  
  username = '';
  password = '';
  rememberMe = false;
  errorMessage = '';
  isLoading = false;

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const request: LoginRequest = {
      userName: this.username,
      password: this.password,
      rememberMe: this.rememberMe
    };

    this.authService.login(request).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Login failed. Please check your credentials.';
      }
    });
  }
}
