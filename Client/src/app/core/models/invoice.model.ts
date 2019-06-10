import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { Currency } from './currency.model';
import { Reservation } from './reservation.model';
import { ModelUtil } from './util';

export class Invoice {
  constructor(public id: number,
              public number: number,
              public date: Date,
              public reservationID: number,
              public reservation: Reservation,
              public currencyID: number,
              public currency: Currency,
              ) {}
}

export enum State {
  Chequeada = 'Chequeada',
  Pagada = 'Pagada'
}

export class InvoiceFilter extends BaseFilter {

  util = new ModelUtil();

  constructor(public searchString: string,
              public reservationID: number,
              public currencyID: number,
              public state: State,
              public date: Date,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    if ( !isNullOrUndefined(this.date) ) {
      hp = hp.set('filter.date', this.util.DateToString(this.date));
    }
    if ( !isNullOrUndefined(this.reservationID) ) {
      hp = hp.set('reservationID', this.reservationID.toString());
    }
    if ( !isNullOrUndefined(this.currencyID) ) {
      hp = hp.set('currencyID', this.currencyID.toString());
    }
    if ( !isNullOrUndefined(this.state) ) {
      hp = hp.set('state', this.state.toString());
    }

    return hp;
  }
}
