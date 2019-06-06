import { Component, OnInit, ViewChild } from '@angular/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import {
  FreeRoom, FreeRoomFilter,
  Agency, Guest, GuestFilter,
  Paginator, Citizenship, Country, Reservation
} from '@app/core/models/core';
import {
  ApiFreeRoomService, ApiAgencyService,
  ApiGuestService, ApiCountryService, ApiCitizenshipService, ApiReservationService
} from '@app/core/services/core';
import { ActivatedRoute } from '@angular/router';
import { SelectionModel} from '@angular/cdk/collections';
import { MatStepper, MatSnackBar } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-new-reservation',
  templateUrl: 'new-reservation.component.html',
  styles: []
})
export class NewReservationComponent implements OnInit {

  @ViewChild(MatStepper) stepper: MatStepper;

  selectedInitialDate = new Date();
  freeRoomsDisplayedColumns = ['number', 'capacity', 'vpn', 'free-days'];
  selectedRoom = 0;
  selectionFreeRooms = new SelectionModel<FreeRoom>(false, []);
  freeRoomsDataSource: CustomDataSource<FreeRoom, FreeRoomFilter>;

  selectedFinalDate = new Date();
  minValidFinalDate = new Date();
  maxValidFinalDate = new Date();

  agencies: Agency[] = [];
  selectedAgencyId = 0;

  details = '';

  selectedGuests: Guest[] = [];

  searchedGuest: Guest = null;

  form: FormGroup;

  countries: Country[] = [];

  citizenships: Citizenship[] = [];

  newGuestForm: FormGroup;

  disableRegisterButton = false;
  disableCreateReservationButton = false;

  constructor(private route: ActivatedRoute,
              private apiFreeRooms: ApiFreeRoomService,
              private apiAgencies: ApiAgencyService,
              private apiGuests: ApiGuestService,
              private apiReservation: ApiReservationService,
              private formBuilder: FormBuilder,
              private snackBar: MatSnackBar) { }

  ngOnInit() {
    // Set data from resolver
    const rd = this.route.snapshot.data.NewReservationPageData;
    if ( rd.error ) {
      console.log('NewReservationComponent: from NewReservationResolver');
      console.log('error: ', rd.error);
    } else {

      this.selectedInitialDate = rd.serverTime;
      this.freeRoomsDataSource = new CustomDataSource<FreeRoom, FreeRoomFilter>(
        this.apiFreeRooms
      );
      this.freeRoomsDataSource.setData(rd.freeRoomsListAndCount.list, rd.freeRoomsListAndCount.cnt);
      this.agencies = rd.agenciesListAndCount.list;
      this.selectedAgencyId = this.agencies[0].id;
      this.details = 'Viaje por turismo';

      this.countries = rd.countriesListAndCount.list;
      this.citizenships = rd.citizenshipsListAndCount.list;
    }

    // Init form
    this.form = this.formBuilder.group({
      initialDate: [this.selectedInitialDate, Validators.required],
      roomId: [this.selectedRoom, Validators.required],
      finalDate: [this.selectedFinalDate, Validators.required],
      agencyId: [this.selectedAgencyId, Validators.required],
      details: [this.details, Validators.required],
    });

    this.newGuestForm = this.formBuilder.group({
      identificationControl: ['', Validators.required],
      nameControl: ['', Validators.required],
      countryControl: [this.countries.length > 0 ? this.countries[0].id : 0,
                        Validators.required],
      citizenshipControl: [this.citizenships.length > 0 ? this.citizenships[0].id : 0,
                        Validators.required],
      phoneControl: ['', Validators.required],
    });
  }

  onSelectInitialDate(date: Date): void {
    this.selectedInitialDate = date;
    this.selectionFreeRooms.clear();
    this.reloadFreeRooms();
  }

  onSelectionFreeRoomsChange(row): void {
    this.selectionFreeRooms.toggle(row);

    if ( !this.selectionFreeRooms.selected || this.selectionFreeRooms.selected.length === 0 ) {
      this.selectedRoom = 0;
    } else {
      this.selectedRoom = this.selectionFreeRooms.selected[0].room.id;

      // Valid values for final dates depend of selectedInititalDate value
      this.minValidFinalDate = new Date(this.selectedInitialDate);
      this.maxValidFinalDate = new Date(this.selectedInitialDate);
      this.minValidFinalDate.setDate( this.minValidFinalDate.getDate() + 1 );
      this.maxValidFinalDate.setDate(
        this.maxValidFinalDate.getDate() +
        this.selectionFreeRooms.selected[0].freeDays
      );
      this.selectedFinalDate = new Date(this.minValidFinalDate);

      console.log('finalDate =', this.selectedFinalDate);
      console.log('minValidFinalDate =', this.minValidFinalDate);
      console.log('maxValidFinalDate =', this.maxValidFinalDate);

      console.log('selectedRoom =', this.selectedRoom);

      // Move to second step
      setTimeout(
        () => { this.stepper.next(); },
        1000
      );
    }
  }

  reloadFreeRooms(): void {
    this.selectionFreeRooms = new SelectionModel<FreeRoom>(false, []);
    this.selectedRoom = 0;
    this.freeRoomsDataSource.load( new FreeRoomFilter(this.selectedInitialDate) );
  }

  onSelectFinalDate(date: Date): void {
    this.selectedFinalDate = date;
  }

  reloadAgencies(): void {
    this.apiAgencies.List().subscribe(
      (agencies) => {
        this.agencies = agencies.list;
      }
    );
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
        return;
    }

    let ok = true;
    for ( let i = 0; i < this.selectedGuests.length; i++ ) {
      if ( this.selectedGuests[i].id === this.searchedGuest.id ) {
        ok = false;
        return;
      }
    }

    if ( ok ) {
      this.selectedGuests.push(this.searchedGuest);
    }
    this.searchedGuest = null;
  }

  onUnselectGuest(id: number): void {
    for ( let i = 0; i < this.selectedGuests.length; i++ ) {
      if ( this.selectedGuests[i].id === id ) {
        this.selectedGuests.splice(i, 1);
        return;
      }
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

  validStep1(): boolean {
    return !isNullOrUndefined(this.selectedInitialDate) && (this.selectedRoom > 0);
  }

  validStep2(): boolean {
    return !isNullOrUndefined(this.selectedFinalDate);
  }

  validStep3(): boolean {
    return (this.form.valid && this.selectedGuests.length > 0);
  }

  onCreateReservation(): void {
    this.disableCreateReservationButton = true;

    const r = {
      id: 0,
      details: this.details,
      initDate: this.selectedInitialDate,
      endDate: this.selectedFinalDate,
      agencyID: this.selectedAgencyId,
      roomID: this.selectedRoom,
      checkIn: false,
      checkOut: false,
      guests: this.selectedGuests
    };

    this.apiReservation.Create(r).subscribe(
      (data) => {
        if (data.succeeded) {
          this.snackBar.open(
            'La reservación fue registrado satisfactoriamente',
            'X',
            {duration: 3000}
          );
        }
        this.disableCreateReservationButton = false;
      }
    );
  }
}
