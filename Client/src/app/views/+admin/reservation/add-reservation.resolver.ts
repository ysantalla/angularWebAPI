import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {
  ApiAgencyService,
  ApiGuestService,
  ApiRoomService,
} from '@app/core/services/core';


@Injectable()
export class AddReservationResolver implements Resolve<any[]> {
  $valueSubject = new BehaviorSubject<any>({});

  $completedSubject = new BehaviorSubject<any>({});

  constructor(private apiAgency: ApiAgencyService,
              private apiGuest: ApiGuestService,
              private apiRoom: ApiRoomService) {
    this.$valueSubject.asObservable().subscribe(
      (v) => {
        if ( v.agencies && v.guests && v.rooms ) {
          console.log('agencies && guests && rooms');
          this.$completedSubject.next(v);
        }
      }
    );
  }

  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    console.log('begin to load add-reservations data');

    this.apiAgency.List().subscribe(
      (agencies) => {
        console.log('loaded agencies');

        const value = this.$valueSubject.value;
        value.agencies = agencies;
        this.$valueSubject.next(value);
      },
      (e) => {
        this.$completedSubject.next(e);
      }
    );

    this.apiGuest.List().subscribe(
      (guests) => {
        console.log('loaded guests');

        const value = this.$valueSubject.value;
        value.guests = guests;
        this.$valueSubject.next(value);
      },
      (e) => {
        this.$completedSubject.next(e);
      }
    );

    this.apiRoom.List().subscribe(
      (rooms) => {
        console.log('loaded rooms');

        const value = this.$valueSubject.value;
        value.rooms = rooms;
        this.$valueSubject.next(value);
      },
      (e) => {
        this.$completedSubject.next(e);
      }
    );

    return this.$completedSubject.asObservable();
  }
}
