import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '@app/core/services/auth.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { MatSnackBar } from '@angular/material';
import { HttpClient } from '@angular/common/http';

import { environment as env } from '@env/environment';
import { ApiGuestService, ApiReservationService } from '@app/core/services/core';
import { GuestFilter, InvoiceFilter, ReservationFilter } from '@app/core/models/core';
import { ApiInvoiceService } from '@app/core/services/api/api-invoice.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit, OnDestroy {

  loading = false;

  chartLabels = ['Chequeada', 'Pagada'];
  private labels$ = new BehaviorSubject([]);

  // options
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = false;
  showXAxisLabel = true;
  xAxisLabel = 'Cantidad';
  showYAxisLabel = false;
  yAxisLabel = 'Color Value';

  schemeType: string = 'ordinal';

  legendTitle = 'Legend';
  legendPosition = 'right';

  tooltipDisabled = false;

  showGridLines = true;
  roundEdges: boolean = true;
  animations: boolean = true;

  showDataLabel = true;
  trimXAxisTicks = true;
  trimYAxisTicks = true;
  rotateXAxisTicks = true;
  maxXAxisTickLength = 16;
  maxYAxisTickLength = 16;

  multi: any[] = [];
  labels: string[] = [];

  reservationCount = 0;

  constructor(
    private snackBar: MatSnackBar,
    private apiReservation: ApiReservationService,
    private apiInvoice: ApiInvoiceService
  ) {}

  ngOnInit(): void {

    this.apiReservation.List(new ReservationFilter(null, null, null, null, null, null, null, null, null)).subscribe(data => {
      this.reservationCount = data.cnt;
    });

    this.apiInvoice.List(new InvoiceFilter(null, null, null, null, null, null, null)).subscribe(data => {

      if (data.list.length > 0) {
        const checkinCount = data.list.filter(x => x.state === 0);
        const checkoutCount = data.list.filter(x => x.state === 1);

        this.multi = [
          {
            name: 'Chequeada',
            value: checkinCount.length
          },
          {
            name: 'Pagada',
            value: checkoutCount.length
          }
        ];
      }
    });

  }

  ngOnDestroy(): void {

  }

}

