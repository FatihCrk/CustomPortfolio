export interface User {
  id: number;
  userName: string;
  email: string;
  firstName?: string;
  lastName?: string;
  roleIds: number[];
  isActive: boolean;
  createdDate: Date;
  updatedDate?: Date;
}

export interface Role {
  id: number;
  name: string;
  description?: string;
  permissions: Permission[];
}

export interface Permission {
  id: number;
  name: string;
  description?: string;
}

export interface LoginRequest {
  userName: string;
  password: string;
  rememberMe?: boolean;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiration: Date;
  user: User;
}

export interface SetupWizardRequest {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}
