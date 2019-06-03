import { Observable, of } from 'rxjs';
import { HttpParams } from '@angular/common/http';

export interface ApiListService<T, TF extends IFilter> {
  List(f?: TF): Observable<ListAndCount<T>>;
}

export class ListAndCount<T> {
  list?: T[];
  cnt?: number;
}

export interface IFilter {
  SetHttpParams(hp: HttpParams): HttpParams;
}
