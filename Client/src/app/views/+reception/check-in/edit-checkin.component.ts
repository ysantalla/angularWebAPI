import { HttpErrorResponse } from '@angular/common/http';
import { Reservation, Agency, Guest, Country, Citizenship, Paginator, GuestFilter, Room } from '@app/core/models/core';
import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef, MatPaginator, MatTableDataSource } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ListAndCount, ApiAgencyService, ApiRoomService, ApiReservationService, ApiGuestService, ApiCountryService, ApiCitizenshipService } from '@app/core/services/core';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-edit-checkin',
  templateUrl: './edit-checkin.component.html',
  styles: [`
    .full-width {
      width: 100%;
    }

    .half-width {
      width: 48%;
      margin: 1%;
    }

    .container {
      width: 100%;
    }

    .item {
      width: 50%;
      padding: 10px;
    }


  `]
})
export class EditCheckInComponent implements OnInit {

  editForm: FormGroup;
  newGuestForm: FormGroup;

  loading = false;

  agencies: Observable<ListAndCount<Agency>>;
  rooms: Observable<ListAndCount<Room>>;

  countries: Observable<ListAndCount<Country>>;
  citizenships: Observable<ListAndCount<Citizenship>>;

  disableRegisterButton = false;
  selectedGuests: Guest[] = [];

  searchedGuest: Guest = null;
  dataSource = new MatTableDataSource<Guest>();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Reservation,
    private formBuilder: FormBuilder,
    private apiAgency: ApiAgencyService,
    private apiRoom: ApiRoomService,
    private apiReservation: ApiReservationService,
    private api: ApiReservationService,
    private apiCountry: ApiCountryService,
    private apiCitizenship: ApiCitizenshipService,
    private apiGuests: ApiGuestService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<EditCheckInComponent>,
  ) { }

  ngOnInit() {

    this.agencies = this.apiAgency.List();
    this.rooms = this.apiRoom.List();

    this.citizenships = this.apiCitizenship.List();
    this.countries = this.apiCountry.List();

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

    this.newGuestForm = this.formBuilder.group({
      identificationControl: ['', Validators.required],
      nameControl: ['', Validators.required],
      countryControl: ['', Validators.required],
      citizenshipControl: ['', Validators.required],
      phoneControl: ['', Validators.required],
    });

    this.selectedGuests = this.data.guestReservations.map((data: any) => {
      return {
        id: data.guest.id,
        name: data.guest.name,
        phone: data.guest.phone,
        identification: data.guest.identification
      };
    });

    this.dataSource.data = this.selectedGuests;
    this.dataSource.paginator = this.paginator;
  }

  onEdit(): void {

    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();

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

          if (!this.editForm.value.checkIn) {
            message = 'no chequeda';
          }
          this.snackBar.open(`Reservación ${message}`, 'X', {duration: 3000});
          this.dialogRef.close();
        } else {
          this.snackBar.open('Opss ha ocurrido un error', 'X', {duration: 3000});
        }
      }, (error: HttpErrorResponse
        ) => {
          this.editForm.enable();
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });
    }
  }

  deleteGuest(reservationId: number, guestId: number): void {

    if (this.selectedGuests.length === 1) {
      this.snackBar.open('Error tiene que haber al menos un huésped en la reservación', 'X', {duration: 3000});
    } else if (this.selectedGuests.length > 1) {
      this.apiReservation.DeleteGuestByReservationId(reservationId, guestId).subscribe(data => {
        if (data.succeeded) {
          this.snackBar.open('Huésped eliminado correctamente', 'X', {duration: 3000});
          this.selectedGuests = this.selectedGuests.filter((x: Guest) => x.id !== guestId);
          this.dataSource.data = this.selectedGuests;
        }
      }, (error: HttpErrorResponse) => {
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });
    } else {
      this.snackBar.open('Opss!!! Error', 'X', {duration: 3000});
    }
  }

  onSearchGuest(identification: string): void {
    console.log('onSearchGuest: ', identification);

    this.searchedGuest = null;
    this.apiGuests.List(
      new GuestFilter(
        '', null, null, identification, new Paginator(0, 10), null
      )
    ).subscribe(
      (guests) => {
        if ( guests.cnt === 1 ) {
          this.searchedGuest = guests.list[0];
        }
      }
    );
  }

  onSelectGuest(): void {
    if ( isNullOrUndefined(this.searchedGuest) ) {
        this.snackBar.open('Error huésped seleccionado no puede ser null', 'X', {duration: 3000});
        this.searchedGuest = null;
        return;
    }

    if (this.selectedGuests.some((x: any) => x.id === this.searchedGuest.id))  {
      this.snackBar.open('Este huésped se encuentra seleccionado', 'X', {duration: 3000});
      this.searchedGuest = null;
    } else {
      this.apiReservation.PutGuestByReservationId(this.data.id, this.searchedGuest.id).subscribe(data => {
        if (data.succeeded) {
          this.snackBar.open('Huésped seleccionado correctamente', 'X', {duration: 3000});
          this.selectedGuests.push(this.searchedGuest);
          this.dataSource.data = this.selectedGuests;
          this.searchedGuest = null;
        }
      }, (error: HttpErrorResponse) => {
        this.snackBar.open(error.error, 'X', {duration: 3000});
        this.searchedGuest = null;
      });
    }
  }

  onRegisterGuest(): void {
    if ( this.newGuestForm.invalid ) {
      return;
    }

    this.disableRegisterButton = true;

    const gf = this.newGuestForm.value;

    const guest: Guest = {
      id: 0,
      identification: gf.identificationControl,
      name: gf.nameControl,
      phone: gf.phoneControl,
      countryID: gf.countryControl,
      citizenshipID: gf.citizenshipControl
    };

    this.apiGuests.Create(guest).subscribe(
      (data) => {
        if (data.succeeded) {
          this.snackBar.open(
            'El huésped fue registrado satisfactoriamente',
            'X',
            {duration: 3000}
          );
          this.disableRegisterButton = false;
          this.newGuestForm.reset();
        }
        this.disableRegisterButton = false;
      },
      (_) => {
        this.disableRegisterButton = false;
      }
    );
  }

}
