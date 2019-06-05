import { ApiReservationService } from './../../../core/services/api/api-reservation.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Room } from './../../../core/models/room.model';
import { ApiRoomService } from './../../../core/services/api/api-room.service';
import { Reservation, Agency } from '@app/core/models/core';
import {Component, Inject, OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ListAndCount, ApiAgencyService } from '@app/core/services/core';

@Component({
  selector: 'app-edit-checkin',
  template: `
    <form [formGroup]="editForm" #f="ngForm" (ngSubmit)="onEdit()" class="form">
      <mat-toolbar color="primary">
        <h2 mat-dialog-title>Editar Reservación</h2>
      </mat-toolbar>
      <div style="overflow: inset!important;">

        <br />

        <h2 class="mat-h2"> Entrada: {{data.initDate | date:'medium'}} <-----> Salida: {{data.endDate | date:'medium'}}</h2>

        <mat-form-field class="full-width">
          <textarea matInput required formControlName="details" placeholder="Detalles"></textarea>
        </mat-form-field>

        <mat-form-field class="half-width">
          <mat-select placeholder="Cambie la agencia" formControlName="agency" required>
            <mat-option *ngFor="let agency of (agencies | async)?.list" [value]="agency.id">{{agency.name}}</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field class="half-width">
          <mat-select placeholder="Cambie el cuarto" formControlName="room" required>
            <mat-option *ngFor="let room of (rooms | async)?.list" [value]="room.id">{{room.number}}</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-slide-toggle formControlName="checkIn"
          class="example-margin">

          {{data.checkIn ? 'checkin' : 'unchecked' }}
        </mat-slide-toggle>

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
export class EditCheckInComponent implements OnInit {

  editForm: FormGroup;

  loading = false;

  agencies: Observable<ListAndCount<Agency>>;
  rooms: Observable<ListAndCount<Room>>;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Reservation,
    private formBuilder: FormBuilder,
    private apiAgency: ApiAgencyService,
    private apiRoom: ApiRoomService,
    private api: ApiReservationService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {

    this.agencies = this.apiAgency.List();
    this.rooms = this.apiRoom.List();

    this.editForm = this.formBuilder.group({
      details: ['', Validators.required],
      agency: ['', Validators.required],
      room: ['', Validators.required],
      checkIn: ['']
    });

    this.editForm.patchValue({
      details: this.data.details,
      agency: this.data.agencyID,
      room: this.data.roomID,
      checkIn: this.data.checkIn
    });

  }

  onEdit(): void {

    this.loading = true;

    if (this.editForm.valid) {
      // this.editForm.disable();

      const reservation  = new Reservation();
      reservation.checkIn = this.editForm.value.checkIn;
      reservation.id = this.data.id;

      reservation.details = this.editForm.value.details;
      reservation.agencyID = this.editForm.value.agency;
      reservation.endDate = this.data.endDate;
      reservation.initDate = this.data.initDate;

      reservation.roomID = this.editForm.value.room;


      this.api.Update(reservation).subscribe(data => {
        if (data.succeeded) {
          let message = 'chequeada';

          if (!this.data.checkIn) {
            message = 'no chequeda';
          }
          this.snackBar.open(`Reservación ${message}`, 'X', {duration: 3000});
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
