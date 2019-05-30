import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { Guest } from './guest.model';
import { Currency } from './currency.model';

export class Invoice {
  constructor(public id: number,
              public number: number,
              public date: Date,
              public identification: string,
              public birthday: string,
              public guestID: number,
              public guest: Guest,
              public currencyID: number,
              public currency: Currency,
              ) {}
}

export class InvoiceFilter extends BaseFilter {
  constructor(public searchString: string,
              public guestID: number,
              public currencyID: number,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    if ( !isNullOrUndefined(this.guestID) ) {
      hp = hp.set('guestID', this.guestID.toString());
    }
    if ( !isNullOrUndefined(this.currencyID) ) {
      hp = hp.set('currencyID', this.currencyID.toString());
    }

    return hp;
  }
}
