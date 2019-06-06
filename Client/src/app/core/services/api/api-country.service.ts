import { ApiCrudService } from './api-crud.service';
import { Country, CountryFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiCountryService extends ApiCrudService<Country, CountryFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/countries';
  }
}
