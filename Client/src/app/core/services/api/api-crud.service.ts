import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment as env } from '@env/environment';
import { isNullOrUndefined } from 'util';

export class ApiCrudService<T extends ObjectWithID, TF extends IFilter> {
  protected url = '';

  constructor(protected http: HttpClient) {
    this.url = env.serverUrl;
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
    return this.http.delete(`${this.url}/${id}`);
  }

  public List(f?: TF): Observable<any> {
    let hp = new HttpParams();
    if (!isNullOrUndefined(f)) {
      hp = f.SetHttpParams(hp);
    }
    return this.http.get(this.url, {params: hp});
  }

  public Count(f: TF): Observable<any> {
    let hp = new HttpParams();
    if (!isNullOrUndefined(f)) {
      hp = f.SetHttpParams(hp);
    }
    return this.http.get(`${this.url}/count`, {params: hp});
  }
}

export interface IFilter {
  SetHttpParams(hp: HttpParams): HttpParams;
}

export interface ObjectWithID {
  id: number;
}
