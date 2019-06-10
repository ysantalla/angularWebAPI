import { Reservation, Guest } from '@app/core/models/core';
import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef, MatTableDataSource, MatPaginator } from '@angular/material';
import { ApiReservationService } from '@app/core/services/core';


@Component({
  selector: 'app-reservation-dialog',
  template: `
      <mat-toolbar color="primary">
        <h2 mat-dialog-title>Ver Reservación</h2>
      </mat-toolbar>
      <div *ngIf="reservation" style="overflow: inset!important;">

        <br />

        <h2 class="mat-h2"> Entrada: {{reservation.initDate | date:'medium'}} <-----> Salida: {{reservation.endDate | date:'medium'}}</h2>

        <h3 class="mat-h3">Agencia: {{reservation.agency.name}} <-----> Cuarto: {{reservation.room.number}}</h3>

        <h3 class="mat-h3">{{reservation.details}}</h3>

        <mat-slide-toggle [disabled]="true" [checked]="reservation.checkIn"
          class="example-margin">
          checkin
        </mat-slide-toggle>

        <mat-slide-toggle [disabled]="true" [checked]="reservation.checkOut"
          class="example-margin">
          checkout
        </mat-slide-toggle>


        <br />
        <br />

        <table mat-table *ngIf="dataSource.data.length > 0" [dataSource]="dataSource">

          <!--- Note that these columns can be defined in any order.
                The actual rendered columns are set as a property on the row definition" -->

          <!-- Name Column -->
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef> Name </th>
            <td mat-cell *matCellDef="let element"> {{element.guest.name}} </td>
          </ng-container>

          <!-- Identification Column -->
          <ng-container matColumnDef="identification">
            <th mat-header-cell *matHeaderCellDef> Identificación </th>
            <td mat-cell *matCellDef="let element"> {{element.guest.identification}} </td>
          </ng-container>

          <!-- Phone Column -->
          <ng-container matColumnDef="phone">
            <th mat-header-cell *matHeaderCellDef> Teléfono </th>
            <td mat-cell *matCellDef="let element"> {{element.guest.phone}} </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="['name', 'identification', 'phone']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['name', 'identification', 'phone'];"></tr>
        </table>

        <mat-paginator [pageSizeOptions]="[5]" [length]="guestCont"></mat-paginator>

      </div>
      <mat-dialog-actions>

        <button mat-button mat-dialog-close>Cancelar</button>

      </mat-dialog-actions>
  `,
  styles: [`
    .full-width {
      width: 100%;
    }

    .half-width {
      width: 48%;
      margin: 1%;
    }


  `]
})
export class ReservationDialogComponent implements OnInit {

  loading = false;

  dataSource = new MatTableDataSource<Guest>();
  reservation: Reservation;

  guestCont = 0;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private api: ApiReservationService
  ) { }

  ngOnInit() {

    this.api.Retrieve(this.data).subscribe(data => {

      if (data.succeeded) {
        this.dataSource.data = data.value.guestReservations;
        this.reservation = data.value;
        this.guestCont = data.value.guestReservations.length;

        this.dataSource.paginator = this.paginator;
      }
    });

  }

}
