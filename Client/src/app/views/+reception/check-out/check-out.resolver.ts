import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiReservationService,
} from '@app/core/services/core';
import { Reservation, ReservationFilter, Paginator } from '@app/core/models/core';


@Injectable()
export class CheckOutResolver implements Resolve<CheckOutPageData> {

  constructor(private apiR: ApiReservationService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<CheckOutPageData> {
    const serverTime = new Date();
    console.log('CheckInResolver: serverTime = ', serverTime);
    const pageSize = 20;

    return forkJoin ([
      this.apiR.List(new ReservationFilter(
        null,
        null,
        serverTime,
        null, null,
        null, null,
        new Paginator(0, pageSize),
        null
      )),
    ])
    .pipe(
        map( ([resApiR]) => {
          return {
            rcollection: resApiR.list,
            count: resApiR.cnt,
            serverTime: serverTime,
            pageSize: pageSize
          };
        }),
        catchError((e) => of({ error: e }))
    );
  }
}

export interface CheckOutPageData {
  rcollection?: Reservation[];
  count?: number;
  pageSize?: number;
  serverTime?: Date;
  error?: Error;
}
