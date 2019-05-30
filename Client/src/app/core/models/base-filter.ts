import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { isNullOrUndefined } from 'util';
import { HttpParams } from '@angular/common/http';

export class BaseFilter {
  constructor(public p: Paginator,
              public ob: OrderBy) {}

  public SetHttpParams(hp: HttpParams): HttpParams {
    if ( !isNullOrUndefined(this.p) ) {
      hp = this.p.SetHttpParams(hp);
    }
    if ( !isNullOrUndefined(this.ob) ) {
      hp = this.ob.SetHttpParams(hp);
    }
    return hp;
  }
}
