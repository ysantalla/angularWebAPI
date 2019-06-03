import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';


@Injectable()
export class ServerTimeResolver implements Resolve<ServerTimeData> {

  constructor() {}

  resolve(route: ActivatedRouteSnapshot): Observable<ServerTimeData> {
    return of({
      date: new Date(),
    });
  }
}

export interface ServerTimeData {
  date: Date;
  error?: Error;
}
