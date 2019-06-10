import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { Paginator, OrderBy, InvoiceFilter, Invoice } from '@app/core/models/core';
import { ApiInvoiceService } from '@app/core/services/api/api-invoice.service';


@Injectable()
export class InvoiceResolver implements Resolve<RangePageData> {

  constructor(private apiInvoice: ApiInvoiceService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<RangePageData> {
    const serverTime = new Date();
    console.log('CheckInResolver: serverTime = ', serverTime);
    const pageSize = 20;

    return forkJoin ([
      this.apiInvoice.List(new InvoiceFilter(
        null,
        null,
        null,
        null,
        null,
        new Paginator(0, pageSize),
        new OrderBy('id', true)
      )),
    ])
    .pipe(
        map( ([resApiR]) => {
          return {
            rcollection: resApiR.list,
            count: resApiR.cnt,
            serverTime: serverTime,
            pageSize: pageSize
          };
        }),
        catchError((e) => of({ error: e }))
    );
  }
}

export interface RangePageData {
  rcollection?: Invoice[];
  count?: number;
  pageSize?: number;
  serverTime?: Date;
  error?: Error;
}
