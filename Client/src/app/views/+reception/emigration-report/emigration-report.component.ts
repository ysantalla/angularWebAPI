import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ApiReservationService } from '@app/core/services/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { Reservation, ReservationFilter, Paginator } from '@app/core/models/core';
import { MatPaginator } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';
import { ReportEmigrationDataSource } from '@app/core/datasources/report-emigration-datasource';

@Component({
  selector: 'app-emigration-report',
  template: `
    <section class="page-container">

      <header class="page-header-container">
        <mat-icon>list</mat-icon>
        <div class="page-header-title">Reporte de Emigración</div>
      </header>

      <br />

      <header class="page-header-container">

        <div class="page-header-title">
          <form [formGroup]="reportForm" #f="ngForm" (ngSubmit)="onReport()" class="form">


            <mat-form-field class="full-padding">
              <input matInput [matDatepicker]="dpid" formControlName="initDate"
                                                    placeholder="Fecha Entrada">
              <mat-datepicker-toggle matSuffix [for]="dpid"></mat-datepicker-toggle>
              <mat-datepicker #dpid></mat-datepicker>
            </mat-form-field>

            <mat-form-field class="full-padding" >
              <input matInput [matDatepicker]="dped" formControlName="endDate" [min]="minEndDate"
                                                placeholder="Fecha Salida">
              <mat-datepicker-toggle matSuffix [for]="dped"></mat-datepicker-toggle>
              <mat-datepicker #dped></mat-datepicker>
            </mat-form-field>

            <button mat-raised-button color="primary" type="submit" [disabled]="!reportForm.valid" aria-label="report">
              <mat-icon>list</mat-icon>
              <span>Reservación</span>
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

                  <ng-container matColumnDef="initDate">
                    <th mat-header-cell *matHeaderCellDef>Fecha de entrada</th>
                    <td mat-cell *matCellDef="let x">
                      {{x.initDate | date}}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="endDate">
                    <th mat-header-cell *matHeaderCellDef>Fecha de salida</th>
                    <td mat-cell *matCellDef="let x">
                      {{x.endDate | date}}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="name">
                    <th mat-header-cell *matHeaderCellDef>Nombre</th>
                    <td mat-cell *matCellDef="let x">
                      {{x.guest.name}}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="identification">
                    <th mat-header-cell *matHeaderCellDef>Identificación</th>
                    <td mat-cell *matCellDef="let x">
                      {{x.guest.identification}}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="phone">
                    <th mat-header-cell *matHeaderCellDef>Teléfono</th>
                    <td mat-cell *matCellDef="let x">
                      {{x.guest.phone}}
                    </td>
                  </ng-container>


                <tr mat-header-row *matHeaderRowDef="['initDate', 'endDate', 'name', 'identification', 'phone']; sticky: true"></tr>
                <tr mat-row *matRowDef="let row; columns: ['initDate', 'endDate', 'name', 'identification', 'phone']"></tr>
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
      width: 40%;
      padding: 5%;
    }

    button {
      margin-left: 5px;
    }
  `]
})
export class EmigrationReportComponent implements OnInit, AfterViewInit {

  reportForm: FormGroup;
  minEndDate = new Date();

  dataSource: ReportEmigrationDataSource<Reservation, ReservationFilter>;
  pageSize = 20;
  selectedDate = new Date();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private api: ApiReservationService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.reportForm = this.formBuilder.group({
      initDate: [''],
      endDate: ['']
    });

    this.reportForm.valueChanges.subscribe(data => {

      if (data.initDate) {
        const newDay = new Date(data.initDate);
        this.minEndDate = new Date (new Date(newDay).setUTCDate(new Date(newDay).getUTCDate() + 1));
      }
    });

    this.dataSource = new ReportEmigrationDataSource<Reservation, ReservationFilter>(this.api);

    // Data from CheckInPageResolver
    const chid = this.route.snapshot.data.reportReservation;
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
      tap(() => this.reloadTable(null, null)),
    ).subscribe();
  }

  reloadTable(initDate: Date, endDate: Date): void {
    this.dataSource.load( new ReservationFilter(
      null,
      null, null,
      initDate,
      endDate,  1, null,
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
      }
      this.reloadTable(initDate, endDate);
    }

  }

  onClear(): void {
    this.reportForm.reset();
  }

}
