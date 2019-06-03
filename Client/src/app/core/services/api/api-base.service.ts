import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment as env } from '@env/environment';

export class ApiBaseService {
  protected url = '';

  constructor(protected http: HttpClient) {
    this.url = env.serverUrl;
  }

  protected he(res: any): Observable<any> {
    if ( res.succeeded ) {
      return of({});
    }
    if ( res.errors ) {
      throw new Error(res.errors);
    }
    throw new Error(res);
  }
}
