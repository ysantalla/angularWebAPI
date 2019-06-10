import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { Paginator, Invoice, InvoiceFilter } from '@app/core/models/core';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';
import { ApiInvoiceService } from '@app/core/services/api/api-invoice.service';
import { State } from '@app/core/models/invoice.model';
import { ReservationDialogComponent } from './reservation-dialog.component';

@Component({
  selector: 'app-invoice',
  template: `
    <div *ngIf="loading">
      <mat-progress-bar color="warn"></mat-progress-bar>
    </div>
    <section class="page-container">

      <header class="page-header-container">
        <mat-icon>list</mat-icon>
        <div class="page-header-title">Facturas</div>
      </header>

      <br />

      <header class="page-header-container">

        <div class="page-header-title">
          <form [formGroup]="reportForm" #f="ngForm" (ngSubmit)="onReport()" class="form">


            <mat-form-field class="full-padding">
              <input matInput [matDatepicker]="dpid" formControlName="initDate"
                                                    placeholder="Escoger Fecha">
              <mat-datepicker-toggle matSuffix [for]="dpid"></mat-datepicker-toggle>
              <mat-datepicker #dpid></mat-datepicker>
            </mat-form-field>

            <button mat-raised-button color="primary" type="submit" [disabled]="!reportForm.valid" aria-label="report">
              <mat-icon>list</mat-icon>
              <span>Facturas</span>
            </button>


            <button mat-raised-button aria-label="clear" (click)="onClear()">
              <mat-icon>clear</mat-icon>
              <span>Limpiar</span>
            </button>


          </form>


        </div>
      </header>

      <div class="check-reservations-container">
        <div class="check-table-container mat-elevation-z8">
            <table mat-table  [dataSource]="dataSource">

              <ng-container matColumnDef="date">
                <th mat-header-cell *matHeaderCellDef>Fecha</th>
                <td mat-cell *matCellDef="let x">
                  {{x.date | date: 'medium'}}
                </td>
              </ng-container>

              <ng-container matColumnDef="number">
                <th mat-header-cell *matHeaderCellDef>Valor x noche</th>
                <td mat-cell *matCellDef="let x">
                  {{x.number}} {{x.currency.symbol}}
                </td>
              </ng-container>

              <ng-container matColumnDef="state">
                <th mat-header-cell *matHeaderCellDef>Estado</th>
                <td mat-cell *matCellDef="let x">
                  {{getState(x.state)}}
                </td>
              </ng-container>

              <ng-container matColumnDef="reservationId">
                <th mat-header-cell *matHeaderCellDef>Reservación</th>
                <td mat-cell *matCellDef="let x">
                  <button type="button" mat-raised-button color="accent" (click)="getReservation(x.reservationID)">
                    <mat-icon>room_service</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="['date', 'number', 'state' , 'reservationId']; sticky: true"></tr>
              <tr mat-row *matRowDef="let row; columns: ['date', 'number', 'state', 'reservationId']"></tr>
            </table>

            <mat-paginator  class="sticky-paginator"
                            [length]="(dataSource.connectCount() | async)" [pageSize]="pageSize">
            </mat-paginator>
        </div>
      </div>

    </section>
  `,
  styles: [`
    .full-padding {
      width: 90%;
      padding: 5%;
    }

    button {
      margin-left: 5px;
    }
  `]
})
export class InvoiceComponent implements OnInit, AfterViewInit {

  reportForm: FormGroup;

  dataSource: CustomDataSource<Invoice, InvoiceFilter>;
  pageSize = 20;
  selectedDate = new Date();
  loading = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private api: ApiInvoiceService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
  ) { }

  ngOnInit() {
    this.reportForm = this.formBuilder.group({
      initDate: ['']
    });

    this.dataSource = new CustomDataSource<Invoice, InvoiceFilter>(this.api);

    // Data from InvoiceResolver
    const chid = this.route.snapshot.data.invoice;
    if ( chid.error ) {
      console.log('CheckInComponent: from CheckInResolver: ', chid.error);
    } else {
      this.dataSource.setData(chid.rcollection, chid.count);
      this.selectedDate = chid.serverTime;
      this.pageSize = chid.pageSize;
    }
  }

  ngAfterViewInit(): void {

    this.paginator.page
    .pipe(
      tap(() => this.reloadTable(null)),
    ).subscribe();
  }

  reloadTable(initDate: Date): void {
    this.dataSource.load( new InvoiceFilter(
      null, null, null, null, initDate,
      new Paginator(
        this.paginator.pageIndex * this.paginator.pageSize,
        this.paginator.pageSize
      ),
      null
    ));

  }

  onReport(): void {

    if (this.reportForm.valid) {
      let endDate = null;
      let initDate = null;
      if (this.reportForm.value.endDate) {
        endDate = this.reportForm.value.endDate;
      }
      if (this.reportForm.value.initDate) {
        initDate = this.reportForm.value.initDate;
        this.reloadTable(initDate);
      }

    }

  }

  onClear(): void {
    this.reportForm.reset();
  }

  getState(key: number): string {
    const states = [State.Chequeada.toString(), State.Pagada.toString()];
    return states[key];
  }

  getReservation(reservationID: number): void {
    const dialogRef = this.dialog.open(ReservationDialogComponent, {
      width: '90%',
      data: reservationID
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
    });

  }

}

