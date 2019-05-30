import { ApiCrudService } from './api-crud.service';
import { Guest, GuestFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiGuestService extends ApiCrudService<Guest, GuestFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/guests';
  }
}
