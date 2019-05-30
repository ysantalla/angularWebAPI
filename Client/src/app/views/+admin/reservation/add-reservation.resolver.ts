import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of as observableOf } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiAgencyService,
  ApiGuestService,
  ApiRoomService,
} from '@app/core/services/core';


@Injectable()
export class AddReservationResolver implements Resolve<any[]> {

  constructor(private apiAgency: ApiAgencyService,
              private apiGuest: ApiGuestService,
              private apiRoom: ApiRoomService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    return forkJoin ([
        this.apiAgency.List(),
        this.apiGuest.List(),
        this.apiRoom.List(),
    ])
    .pipe(
        map(([agencies, guests, rooms]) => {
          return {
            agencies: agencies,
            guests: guests,
            rooms: rooms
          };
        }),
        catchError((e) => {
          console.log('error =', e);
          return observableOf({});
        })
    );
  }
}
