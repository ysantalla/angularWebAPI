import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

export class Room {
  constructor(public id: number,
              public number: string,
              public capacity: number,
              public bedCont: number,
              public enable: boolean) {}
}

export class RoomFilter extends BaseFilter {
  constructor(public searchString: string,
              public capacity: number,
              public bedCont: number,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    if ( !isNullOrUndefined(this.capacity) ) {
      hp = hp.set('filter.capacity', this.capacity.toString());
    }
    if ( !isNullOrUndefined(this.bedCont) ) {
      hp = hp.set('filter.bedCont', this.bedCont.toString());
    }
    return hp;
  }
}
