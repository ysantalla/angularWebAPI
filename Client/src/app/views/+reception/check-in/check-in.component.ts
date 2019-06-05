import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Reservation, ReservationFilter, Paginator } from '@app/core/models/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { ApiReservationService } from '@app/core/services/core';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { tap, filter } from 'rxjs/operators';
import { EditCheckInComponent } from './edit-checkin.component';
import { AutofillMonitor } from '@angular/cdk/text-field';

@Component({
  selector: 'app-check-in',
  templateUrl: 'check-in.component.html',
  styles: []
})
export class CheckInComponent implements OnInit, AfterViewInit {

  dataSource: CustomDataSource<Reservation, ReservationFilter>;
  pageSize = 20;
  selectedDate = new Date();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private api: ApiReservationService,
              private route: ActivatedRoute,
              private dialog: MatDialog,
              private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.dataSource = new CustomDataSource<Reservation, ReservationFilter>(this.api);

    // Data from CheckInPageResolver
    const chid = this.route.snapshot.data.CheckInPageData;
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
      tap(() => this.reloadTable()),
    ).subscribe();
  }

  onSelectDate(date: Date): void {
    this.selectedDate = date;
    this.reloadTable();
  }

  reloadTable(): void {
    this.dataSource.load( new ReservationFilter(
      null,
      this.selectedDate,
      null, null, null,
      new Paginator(
        this.paginator.pageIndex * this.paginator.pageSize,
        this.paginator.pageSize
      ),
      null
    ));
  }

  onShowSettings(reservation: Reservation): void {
    console.log('onShowSetting: reservationID =', reservation);
    const dialogRef = this.dialog.open(EditCheckInComponent, {
      data: reservation
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);
      }
    });
  }

  // checkIn($event: any) {

  //   const reservation  = new Reservation();
  //   reservation.checkIn = $event.checked;
  //   reservation.checkOut = $event.source.id.checkOut;
  //   reservation.id = $event.source.id.id;

  //   reservation.details = $event.source.id.details;
  //   reservation.agencyID = $event.source.id.agencyID;
  //   reservation.endDate = $event.source.id.endDate;
  //   reservation.initDate = $event.source.id.initDate;

  //   reservation.roomID = $event.source.id.roomID;

  //   this.api.Update(reservation).subscribe(data => {
  //     if (data.succeeded) {
  //       let message = 'chequeada';

  //       if (!$event.checked) {
  //         message = 'no chequeda';
  //       }
  //       this.snackBar.open(`Reservación ${message}`, 'X', {duration: 3000});
  //     } else {
  //       this.snackBar.open('Opss ha ocurrido un error', 'X', {duration: 3000});
  //     }
  //   }, (error: HttpErrorResponse) => {
  //     this.snackBar.open(error.message, 'X', {duration: 3000});
  //   });
  // }
}
