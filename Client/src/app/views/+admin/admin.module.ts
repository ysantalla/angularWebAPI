import { CitizenhipsResolver } from './resolvers/citizenhips.resolver';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { RoleGuard } from '@app/core/guards/role.guard';


import { ListCountryComponent } from './country/list-country.component';
import { AddCountryComponent } from './country/add-country.component';
import { EditCountryComponent } from './country/edit-country.component';

import { ListRoomComponent } from './room/list-room.component';
import { AddRoomComponent } from './room/add-room.component';
import { EditRoomComponent } from './room/edit-room.component';

import { EditUserComponent } from './user/edit-user.component';
import { AddUserComponent } from './user/add-user.component';
import { ListUserComponent } from './user/list-user.component';
import { UserAddRoleComponent } from './user/user-add-role.component';

import { CountryResolver } from './resolvers/country.resolver';
import { UserResolver } from './resolvers/user.resolver';
import { AddAgencyComponent } from './agency/add-agency.component';
import { EditAgencyComponent } from './agency/edit-agency.component';
import { ListAgencyComponent } from './agency/list-agency.component';
import { AgencyResolver } from './resolvers/agency.resolver';
import { RoomResolver } from './resolvers/room.resolver';
import { EditCitizenhipsComponent } from './citizenhips/edit-citizenhips.component';
import { AddCitizenhipsComponent } from './citizenhips/add-citizenhips.component';
import { ListCitizenhipsComponent } from './citizenhips/list-citizenhips.component';

import {
  Paginator,
  ReservationFilter
} from '@app/core/models/core';
import { EditCurrencyComponent } from './currency/edit-currency.component';
import { AddCurrencyComponent } from './currency/add-currency.component';
import { ListCurrencyComponent } from './currency/list-currency.component';
import { CurrencyResolver } from './resolvers/currency.resolver';

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
    path: 'currency',
    component: ListCurrencyComponent,
    data: {title: 'Listado de Monedas', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: CurrencyResolver
    }
  },
  {
    path: 'currency/add',
    component: AddCurrencyComponent,
    data: {title: 'Adicionar Moneda', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'currency/edit/:id',
    component: EditCurrencyComponent,
    data: {title: 'Editar Moneda', expectedRole: ['Admin']},
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
  {
    path: 'rooms',
    component: ListRoomComponent,
    data: {title: 'Listado de Cuartos', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: RoomResolver
    }
  },
  {
    path: 'room/add',
    component: AddRoomComponent,
    data: {title: 'Adicionar Cuarto', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'room/edit/:id',
    component: EditRoomComponent,
    data: {title: 'Editar Cuarto', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },

  {
    path: 'citizenhips',
    component: ListCitizenhipsComponent,
    data: {title: 'Listado de Ciudadanía', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
    resolve: {
      data: CitizenhipsResolver
    }
  },
  {
    path: 'citizenhips/add',
    component: AddCitizenhipsComponent,
    data: {title: 'Adicionar Ciudadanía', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  },
  {
    path: 'citizenhips/edit/:id',
    component: EditCitizenhipsComponent,
    data: {title: 'Editar Ciudadanía', expectedRole: ['Admin']},
    canActivate: [RoleGuard],
  }
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
    ListUserComponent, AddUserComponent, EditUserComponent, UserAddRoleComponent,
    ListAgencyComponent, AddAgencyComponent, EditAgencyComponent,
    ListCountryComponent, AddCountryComponent, EditCountryComponent,
    ListCurrencyComponent, AddCurrencyComponent, EditCurrencyComponent,
    ListCitizenhipsComponent, AddCitizenhipsComponent, EditCitizenhipsComponent,
    ListRoomComponent, AddRoomComponent, EditRoomComponent
  ],
  providers: [
    UserResolver,
    AgencyResolver,
    CountryResolver,
    RoomResolver,
    CitizenhipsResolver,
    CurrencyResolver
  ]
})
export class AdminModule { }
