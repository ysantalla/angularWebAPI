import { Paginator } from './paginator.model';
import { OrderBy } from './orderby.model';
import { BaseFilter } from './base-filter';
import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { Country } from './country.model';
import { Citizenship } from './citizenship.model';

export interface Guest {
    id: number;
    name?: string;
    phone?: string;
    identification?: string;
    birthday?: string;
    countryID?: number;
    country?: Country;
    citizenshipID?: number;
    citizenship?: Citizenship;
}

export class GuestFilter extends BaseFilter {
  constructor(public searchString: string,
              public countryID: number,
              public citizenshipID: number,
              p: Paginator,
              ob: OrderBy) {
    super(p, ob);
  }

  public SetHttpParams(hp: HttpParams): HttpParams {
    hp = super.SetHttpParams(hp);

    if ( !isNullOrUndefined(this.searchString) && this.searchString !== '' ) {
      hp = hp.set('filter.searchString', this.searchString);
    }
    if ( !isNullOrUndefined(this.countryID) ) {
      hp = hp.set('countryID', this.countryID.toString());
    }
    if ( !isNullOrUndefined(this.citizenshipID) ) {
      hp = hp.set('citizenshipID', this.citizenshipID.toString());
    }

    return hp;
  }
}
