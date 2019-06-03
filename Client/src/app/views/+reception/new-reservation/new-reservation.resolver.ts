import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiFreeRoomService, ApiAgencyService, ListAndCount,
} from '@app/core/services/core';
import { Reservation, FreeRoomFilter, FreeRoom, Agency } from '@app/core/models/core';


@Injectable()
export class NewReservationResolver implements Resolve<NewReservationPageData> {

  constructor(private apiFreeRooms: ApiFreeRoomService,
              private apiAgencies: ApiAgencyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<NewReservationPageData> {
    const serverTime = new Date();
    console.log('NewReservationResolver: serverTime = ', serverTime);

    return forkJoin ([
      this.apiFreeRooms.List(new FreeRoomFilter(serverTime)),
      this.apiAgencies.List(),
    ])
    .pipe(
        map( ([resFreeRooms, resAgencies]) => {
          return {
            serverTime: serverTime,
            freeRoomsListAndCount: resFreeRooms,
            agenciesListAndCount: resAgencies,
          };
        }),
        catchError((e) => of({ error: e }))
    );
  }
}

export interface NewReservationPageData {
  serverTime?: Date;
  freeRoomsListAndCount?: ListAndCount<FreeRoom>;
  agenciesListAndCount?: ListAndCount<Agency>;
  error?: Error;
}
