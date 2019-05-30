import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

export class Citizenship {
  constructor(public id: number,
              public name: string) {}
}

export class CitizenshipFilter extends BaseFilter {
  constructor(public searchString: string,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    return hp;
  }
}
