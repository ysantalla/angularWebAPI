import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {
  ApiFreeRoomService,
  ApiAgencyService,
  ApiCountryService,
  ApiCitizenshipService,
  ListAndCount,
} from '@app/core/services/core';
import { FreeRoomFilter, FreeRoom, Agency, Country, Citizenship } from '@app/core/models/core';


@Injectable()
export class NewReservationResolver implements Resolve<NewReservationPageData> {

  constructor(private apiFreeRooms: ApiFreeRoomService,
              private apiAgencies: ApiAgencyService,
              private apiCountries: ApiCountryService,
              private apiCitizenships: ApiCitizenshipService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<NewReservationPageData> {
    const serverTime = new Date();
    console.log('NewReservationResolver: serverTime = ', serverTime);

    return forkJoin ([
      this.apiFreeRooms.List(new FreeRoomFilter(serverTime)),
      this.apiAgencies.List(),
      this.apiCountries.List(),
      this.apiCitizenships.List(),
    ])
    .pipe(
        map( ([resFreeRooms, resAgencies, resCountries, resCitizenships]) => {
          return {
            serverTime: serverTime,
            freeRoomsListAndCount: resFreeRooms,
            agenciesListAndCount: resAgencies,
            citizenshipsListAndCount: resCitizenships,
            countriesListAndCount: resCountries,
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
  countriesListAndCount?: ListAndCount<Country>;
  citizenshipsListAndCount?: ListAndCount<Citizenship>;
  error?: Error;
}
