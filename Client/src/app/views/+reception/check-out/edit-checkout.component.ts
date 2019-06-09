import { ApiReservationService } from '../../../core/services/api/api-reservation.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Reservation, Guest } from '@app/core/models/core';
import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef, MatTableDataSource, MatPaginator } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
  selector: 'app-edit-checkout',
  template: `
    <form [formGroup]="editForm" #f="ngForm" (ngSubmit)="onEdit()" class="form">
      <mat-toolbar color="primary">
        <h2 mat-dialog-title>Editar estado</h2>
      </mat-toolbar>
      <div style="overflow: inset!important;">

        <br />

        <h2 class="mat-h2"> Entrada: {{data.initDate | date:'medium'}} <-----> Salida: {{data.endDate | date:'medium'}}</h2>

        <h3 class="mat-h3">Agencia: {{data.agency.name}} <-----> Cuarto: {{data.room.number}}</h3>

        <h3 class="mat-h3">{{data.details}}</h3>

        <mat-slide-toggle formControlName="checkOut"
          class="example-margin">
          checkout
        </mat-slide-toggle>


        <br />
        <br />

        <table mat-table [dataSource]="dataSource">

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

        <mat-paginator [pageSizeOptions]="[5]" [length]="data.guestReservations.length"></mat-paginator>

      </div>
      <mat-dialog-actions>
        <!-- The mat-dialog-close directive optionally accepts a value as a result for the dialog. -->
        <button mat-raised-button color="primary" type="submit" [disabled]="!editForm.valid" aria-label="edit">
          <mat-icon>mode_edit</mat-icon>
          <span>Reservación</span>
        </button>

        <button mat-button mat-dialog-close>Cancelar</button>


      </mat-dialog-actions>
    </form>
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
export class EditCheckOutComponent implements OnInit {

  editForm: FormGroup;
  loading = false;

  dataSource = new MatTableDataSource<Guest>();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Reservation,
    private formBuilder: FormBuilder,
    private api: ApiReservationService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<EditCheckOutComponent>,
  ) { }

  ngOnInit() {

    this.editForm = this.formBuilder.group({
      checkOut: ['']
    });

    this.editForm.patchValue({
      checkOut: this.data.checkOut
    });

    this.dataSource.data = this.data.guestReservations;

    this.dataSource.paginator = this.paginator;

  }

  onEdit(): void {

    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();
      this.data.checkOut = this.editForm.value.checkOut;

      this.api.Update(this.data).subscribe(data => {
        if (data.succeeded) {
          let message = 'chequeada';

          if (!this.editForm.value.checkOut) {
            message = 'no chequeda';
          }
          this.snackBar.open(`Reservación ${message}`, 'X', {duration: 3000});
          this.dialogRef.close();
        } else {
          this.snackBar.open('Opss ha ocurrido un error', 'X', {duration: 3000});
        }
      }, (error: HttpErrorResponse
        ) => {
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });
    }
  }

}
