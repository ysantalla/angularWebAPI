import { HttpParams, HttpClient } from '@angular/common/http';
import { ApiListService, ListAndCount } from './api-list.service';
import { isNullOrUndefined } from 'util';
import { ApiBaseService } from './api-base.service';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { FreeRoom, FreeRoomFilter } from '@app/core/models/core';


@Injectable({
  providedIn: 'root'
})
export class ApiFreeRoomService extends ApiBaseService
                  implements ApiListService<FreeRoom, FreeRoomFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/rooms/free';
  }

  public List(f?: FreeRoomFilter): Observable<ListAndCount<FreeRoom>> {
    let hp = new HttpParams();
    if (!isNullOrUndefined(f)) {
      hp = f.SetHttpParams(hp);
    }
    return this.http.get<any>(this.url, {params: hp})
    .pipe(
      map( res => {
        this.he(res);
        return { list: res.value, cnt: res.countItems };
      }),
      catchError( (e) => this.he(e) ),
    );
  }
}
