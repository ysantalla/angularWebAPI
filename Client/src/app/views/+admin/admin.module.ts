import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { RoleGuard } from '@app/core/guards/role.guard';


import { ListCountryComponent } from './country/list-country.component';
import { AddCountryComponent } from './country/add-country.component';
import { EditCountryComponent } from './country/edit-country.component';
import { CountryResolver } from './resolvers/country.resolver';

const routes: Routes = [
  {
    path: 'country/list',
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
  declarations: [ListCountryComponent, AddCountryComponent, EditCountryComponent],
  providers: [CountryResolver]
})
export class AdminModule { }
