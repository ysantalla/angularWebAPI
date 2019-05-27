import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Injectable()
export class CountryResolver implements Resolve<any[]> {
  constructor(private httpClient: HttpClient) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any[]> {
    return this.httpClient.get<any>(
      `${env.serverUrl}/countries?filter.SearchString=&paginator.offset=0&paginator.limit=2&orderBy.by=name&orderBy.desc=false`);
  }
}
