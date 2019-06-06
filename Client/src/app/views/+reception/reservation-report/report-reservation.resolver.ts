import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiReservationService,
} from '@app/core/services/core';
import { Reservation, ReservationFilter, Paginator, OrderBy } from '@app/core/models/core';


@Injectable()
export class ReportReservationResolver implements Resolve<RangePageData> {

  constructor(private apiR: ApiReservationService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<RangePageData> {
    const serverTime = new Date();
    console.log('CheckInResolver: serverTime = ', serverTime);
    const pageSize = 20;

    return forkJoin ([
      this.apiR.List(new ReservationFilter(
        null,
        null,
        null,
        null, null, null, null,
        new Paginator(0, pageSize),
        new OrderBy('id', true)
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

export interface RangePageData {
  rcollection?: Reservation[];
  count?: number;
  pageSize?: number;
  serverTime?: Date;
  error?: Error;
}
