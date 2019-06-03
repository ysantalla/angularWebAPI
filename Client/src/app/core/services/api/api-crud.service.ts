import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { isNullOrUndefined } from 'util';
import { map, catchError } from 'rxjs/operators';
import { ApiListService, ListAndCount, IFilter } from './api-list.service';
import { ApiBaseService } from './api-base.service';

export class ApiCrudService<T extends ObjectWithID, TF extends IFilter>
                extends ApiBaseService implements ApiListService<T, TF> {
  constructor(http: HttpClient) {
    super(http);
  }

  public Create(o: T): Observable<any> {
    return this.http.post(this.url, o);
  }

  public Retrieve(id: number): Observable<any> {
    return this.http.get(`${this.url}/${id}`);
  }

  public Update(o: T): Observable<any> {
    return this.http.patch(`${this.url}/${o.id}`, o);
  }

  public Delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }

  public List(f?: TF): Observable<ListAndCount<T>> {
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

  public Count(f: TF): Observable<number> {
    let hp = new HttpParams();
    if (!isNullOrUndefined(f)) {
      hp = f.SetHttpParams(hp);
    }
    return this.http.get<any>(`${this.url}/count`, {params: hp})
    .pipe(
      map( res => {
        this.he(res);
        return Number(res.value);
      }),
      catchError( (e) => this.he(e) ),
    );
  }
}

export interface ObjectWithID {
  id: number;
}
