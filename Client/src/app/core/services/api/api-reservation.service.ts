import { ApiCrudService } from './api-crud.service';
import { Reservation, ReservationFilter, Guest } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiReservationService extends ApiCrudService<Reservation, ReservationFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/reservations';
  }

  public CreateReservation(reservation: Reservation, guest: Guest[]): Observable<any> {
    return this.http.post(this.url, {reservation: reservation, guest: guest});
  }

  public DeleteGuestByReservationId(reservationId: number, guestId: number): Observable<any> {
    return this.http.delete(`${this.url}/${reservationId}/guests/${guestId}`);
  }

  public PutGuestByReservationId(reservationId: number, guestId: number): Observable<any> {
    return this.http.put(`${this.url}/${reservationId}/guests/${guestId}`, {});
  }

}
