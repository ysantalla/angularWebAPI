import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { Guest } from './guest.model';
import { Agency } from './agency.model';
import { Room } from './room.model';

export class Reservation {
  id: number;
  details?: string;
  initDate?: Date;
  endDate?: Date;
  guestID?: number;
  guest?: Guest;
  agencyID?: number;
  agency?: Agency;
  roomID?: number;
  room?: Room;
}

export class ReservationFilter extends BaseFilter {
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
