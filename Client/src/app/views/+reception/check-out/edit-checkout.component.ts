import { ApiReservationService } from '../../../core/services/api/api-reservation.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Room } from '../../../core/models/room.model';
import { ApiRoomService } from '../../../core/services/api/api-room.service';
import { Reservation, Agency } from '@app/core/models/core';
import {Component, Inject, OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ListAndCount, ApiAgencyService } from '@app/core/services/core';


@Component({
  selector: 'app-edit-checkout',
  template: `
    <form [formGroup]="editForm" #f="ngForm" (ngSubmit)="onEdit()" class="form">
      <mat-toolbar color="primary">
        <h2 mat-dialog-title>Editar Reservación</h2>
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
        <br />

        <mat-chip-list class="mat-chip-list-stacked" aria-label="">
          <mat-chip *ngFor="let item of data.guestReservations">
            {{item.guest.name}} --- {{item.guest.identification}}
          </mat-chip>
        </mat-chip-list>

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
