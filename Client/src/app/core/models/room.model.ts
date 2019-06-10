import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { IFilter } from '../services/core';
import { ModelUtil } from './util';
import { Currency } from './currency.model';

export class Room {
  constructor(public id: number,
              public number: string,
              public capacity: number,
              public bedCont: number,
              public VPN: number,
              public enable: boolean,
              public currencyID: number,
              public currency: Currency) {}
}

export class RoomFilter extends BaseFilter {
  constructor(public searchString: string,
              public capacity: number,
              public bedCont: number,
              public VPN: number,
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
    if ( !isNullOrUndefined(this.VPN) ) {
      hp = hp.set('filter.VPN', this.VPN.toString());
    }
    if ( !isNullOrUndefined(this.VPN) ) {
      hp = hp.set('filter.VPN', this.VPN.toString());
    }
    return hp;
  }
}

export class FreeRoom {
  constructor(public room: Room,
              public freeDays: number) {}
}

export class FreeRoomFilter implements IFilter {
  util = new ModelUtil();

  constructor(public initialDate: Date) {}

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = hp.set('initialDate', this.util.DateToString(this.initialDate));
    return hp;
  }
}
