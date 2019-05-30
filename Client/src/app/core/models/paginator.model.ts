import { isNullOrUndefined } from 'util';
import { HttpParams } from '@angular/common/http';

export class Paginator {
  constructor(public offset: number,
              public limit: number) {}

  public SetHttpParams(hp: HttpParams): HttpParams {
    if ( !isNullOrUndefined(this.offset) ) {
      hp = hp.set('paginator.offset', this.offset.toString());
    }
    if ( !isNullOrUndefined(this.limit) ) {
      hp = hp.set('paginator.limit', this.limit.toString());
    }
    return hp;
  }
}
