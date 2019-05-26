import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { RoleGuard } from '@app/core/guards/role.guard';


import { ListCountryComponent } from './country/list-country.component';
import { AddCountryComponent } from './country/add-country.component';
import { EditCountryComponent } from './country/edit-country.component';

import { EditRoleComponent } from './role/edit-role.component';
import { AddRoleComponent } from './role/add-role.component';
import { ListRoleComponent } from './role/list-role.component';
import { EditUserComponent } from './user/edit-user.component';
import { AddUserComponent } from './user/add-user.component';
import { ListUserComponent } from './user/list-user.component';
import { UserAddRoleComponent } from './user/user-add-role.component';

import { CountryResolver } from './resolvers/country.resolver';
import { RoleResolver } from './resolvers/role.resolver';
import { UserResolver } from './resolvers/user.resolver';
import { AddAgencyComponent } from './agency/add-agency.component';
import { EditAgencyComponent } from './agency/edit-agency.component';
import { ListAgencyComponent } from './agency/list-agency.component';
import { AgencyResolver } from './resolvers/agency.resolver';

const routes: Routes = [
  {
    path: 'country',
    component: ListCountryComponent,
    data: {title: 'Listado de Paises', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: CountryResolver
    }
  },
  {
    path: 'country/add',
    component: AddCountryComponent,
    data: {title: 'Adicionar País', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'country/edit/:id',
    component: EditCountryComponent,
    data: {title: 'Editar País', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'role',
    component: ListRoleComponent,
    data: {title: 'Listado de Roles', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: RoleResolver
    }
  },
  {
    path: 'role/add',
    component: AddRoleComponent,
    data: {title: 'Adicionar Rol', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'role/edit/:id',
    component: EditRoleComponent,
    data: {title: 'Editar Rol', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'user',
    component: ListUserComponent,
    data: {title: 'Listado de Usuarios', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: UserResolver
    }
  },
  {
    path: 'user/add',
    component: AddUserComponent,
    data: {title: 'Adicionar Usuario', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'user/add_role/:id',
    component: UserAddRoleComponent,
    data: {title: 'Adicionar Rol a Usuario', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'user/edit/:id',
    component: EditUserComponent,
    data: {title: 'Editar Usuario', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'agency',
    component: ListAgencyComponent,
    data: {title: 'Listado de Agencia', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: AgencyResolver
    }
  },
  {
    path: 'agency/add',
    component: AddAgencyComponent,
    data: {title: 'Adicionar Agencia', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'agency/edit/:id',
    component: EditAgencyComponent,
    data: {title: 'Editar Agencia', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    ListAgencyComponent, AddAgencyComponent, EditAgencyComponent,
    ListCountryComponent, AddCountryComponent, EditCountryComponent,
    ListRoleComponent, AddRoleComponent, EditRoleComponent,
    ListUserComponent, AddUserComponent, EditUserComponent, UserAddRoleComponent
  ],
  providers: [CountryResolver, RoleResolver, UserResolver, AgencyResolver]
})
export class AdminModule { }
