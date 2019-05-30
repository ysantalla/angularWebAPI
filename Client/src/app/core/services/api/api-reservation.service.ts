import { ApiCrudService } from './api-crud.service';
import { Reservation, ReservationFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiReservationService extends ApiCrudService<Reservation, ReservationFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/reservations';
  }
}
