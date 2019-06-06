import { ApiCrudService } from './api-crud.service';
import { Citizenship, CitizenshipFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiCitizenshipService extends ApiCrudService<Citizenship, CitizenshipFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/citizenships';
  }
}
