import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Reservation, ReservationFilter, Paginator } from '@app/core/models/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { ApiReservationService } from '@app/core/services/core';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { tap, filter } from 'rxjs/operators';
import { EditCheckOutComponent } from './edit-checkout.component';

@Component({
  selector: 'app-check-out',
  templateUrl: 'check-out.component.html',
  styles: []
})
export class CheckOutComponent implements OnInit, AfterViewInit {

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
    const chid = this.route.snapshot.data.CheckOutPageData;
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
      null,
      this.selectedDate,
      null, null,
      null, null,
      new Paginator(
        this.paginator.pageIndex * this.paginator.pageSize,
        this.paginator.pageSize
      ),
      null
    ));
  }

  onShowSettings(reservation: Reservation): void {
    console.log('onShowSetting: reservationID =', reservation);
    const dialogRef = this.dialog.open(EditCheckOutComponent, {
      data: reservation,
      width: '90%'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.reloadTable();
    });
  }

}
