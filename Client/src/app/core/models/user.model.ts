export interface User {
  id?: string;
  firstname: string;
  lastname: string;
  email: string;
  userName: string;
  roles: string[];
}

export interface Role {
  id?: string;
  name: string;
}
