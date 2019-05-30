import { ApiCrudService } from './api-crud.service';
import { Agency, AgencyFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiAgencyService extends ApiCrudService<Agency, AgencyFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/agencies';
  }
}
