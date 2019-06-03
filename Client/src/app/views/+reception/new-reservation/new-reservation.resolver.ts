import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiFreeRoomService,
} from '@app/core/services/core';
import { Reservation, FreeRoomFilter, FreeRoom } from '@app/core/models/core';


@Injectable()
export class NewReservationResolver implements Resolve<NewReservationPageData> {

  constructor(private apiFreeRooms: ApiFreeRoomService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<NewReservationPageData> {
    const serverTime = new Date();
    console.log('NewReservationResolver: serverTime = ', serverTime);

    return forkJoin ([
      this.apiFreeRooms.List(new FreeRoomFilter(serverTime)),
    ])
    .pipe(
        map( ([resFreeRooms]) => {
          return {
            freeRooms: resFreeRooms.list,
            freeRoomsCnt: resFreeRooms.cnt,
            serverTime: serverTime
          };
        }),
        catchError((e) => of({ error: e }))
    );
  }
}

export interface NewReservationPageData {
  freeRooms?: FreeRoom[];
  freeRoomsCnt?: number;
  serverTime?: Date;
  error?: Error;
}
