import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Injectable()
export class RoleResolver implements Resolve<any[]> {
  constructor(private httpClient: HttpClient) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any[]> {
    return this.httpClient.get<any>(`${env.serverUrl}/Role/List?searchString=&pageIndex=1&pageSize=20&sortOrder=name_asc`);
  }
}
