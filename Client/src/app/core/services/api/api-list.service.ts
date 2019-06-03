import { Observable, of } from 'rxjs';
import { HttpParams, HttpClient } from '@angular/common/http';
import { ApiBaseService } from './api-base.service';
import { isNullOrUndefined } from 'util';
import { map, catchError } from 'rxjs/operators';

export interface IApiListService<T, TF extends IFilter> {
  List(f?: TF): Observable<ListAndCount<T>>;
}

export class ListAndCount<T> {
  list?: T[];
  cnt?: number;
}

export interface IFilter {
  SetHttpParams(hp: HttpParams): HttpParams;
}

export class ApiListService<T, TF extends IFilter> extends ApiBaseService
        implements IApiListService<T, TF> {

  constructor(http: HttpClient) {
    super(http);
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

  public SetURL(url: string) {
    this.url = url;
  }
}
