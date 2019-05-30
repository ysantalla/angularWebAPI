import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiReservationService } from '@app/core/services/core';
import { ReservationFilter, Paginator } from '@app/core/models/core';


@Injectable()
export class ReservationListResolver implements Resolve<any[]> {
  constructor(private httpClient: HttpClient,
              private api: ApiReservationService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    return this.api.List(route.data.filter);
  }
}
