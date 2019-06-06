import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { Guest } from './guest.model';
import { Agency } from './agency.model';
import { Room } from './room.model';
import { ModelUtil } from './util';

export class Reservation {
  id: number;
  details?: string;
  initDate?: Date;
  endDate?: Date;
  guests: Guest[];
  guestReservations?: GuestReservations[];
  agencyID?: number;
  agency?: Agency;
  roomID?: number;
  room?: Room;
  checkIn?: boolean;
  checkOut?: boolean;
}

export class  GuestReservations {
  guest: Guest;
  reservationId: number;
  guestId: number;
}

export class ReservationFilter extends BaseFilter {
  util = new ModelUtil();

  constructor(public searchString: string,
              public checkInDate: Date,
              public checkOutDate: Date,
              public InDate: Date,
              public OutDate: Date,
              public checkInState: any,
              public checkOutState: any,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    if ( !isNullOrUndefined(this.checkInDate)) {
      hp = hp.set('filter.checkInDate', this.util.DateToString(this.checkInDate));
    }
    if ( !isNullOrUndefined(this.checkOutDate) ) {
      hp = hp.set('filter.checkOutDate', this.util.DateToString(this.checkOutDate));
    }

    if ( !isNullOrUndefined(this.InDate)) {
      hp = hp.set('filter.InDate', this.util.DateToString(this.InDate));
    }
    if ( !isNullOrUndefined(this.OutDate) ) {
      hp = hp.set('filter.OutDate', this.util.DateToString(this.OutDate));
    }
    if ( !isNullOrUndefined(this.checkInState) && this.checkInState !== '' ) {
      hp = hp.set('filter.checkInState', this.checkInState);
    }
    if ( !isNullOrUndefined(this.checkOutState) && this.checkOutState !== '' ) {
      hp = hp.set('filter.checkOutState', this.checkOutState);
    }
    return hp;
  }
}
