import { Component, OnInit, ViewChild } from '@angular/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { FreeRoom, FreeRoomFilter, Agency } from '@app/core/models/core';
import { ApiFreeRoomService, ApiAgencyService } from '@app/core/services/core';
import { ActivatedRoute } from '@angular/router';
import { SelectionModel} from '@angular/cdk/collections';
import { MatStepper } from '@angular/material';

@Component({
  selector: 'app-new-reservation',
  templateUrl: 'new-reservation.component.html',
  styles: []
})
export class NewReservationComponent implements OnInit {

  @ViewChild(MatStepper) stepper: MatStepper;

  selectedInitialDate = new Date();
  freeRoomsDisplayedColumns = ['number', 'capacity', 'vpn', 'free-days'];
  selectedRoom = '';
  selectionFreeRooms = new SelectionModel<FreeRoom>(false, []);
  freeRoomsDataSource: CustomDataSource<FreeRoom, FreeRoomFilter>;

  selectedFinalDate = new Date();
  minValidFinalDate = new Date();
  maxValidFinalDate = new Date();

  agencies: Agency[] = [];

  constructor(private route: ActivatedRoute,
              private apiFreeRooms: ApiFreeRoomService,
              private apiAgencies: ApiAgencyService) { }

  ngOnInit() {
    // Set data from resolver
    const rd = this.route.snapshot.data.NewReservationPageData;
    if ( rd.error ) {
      console.log('NewReservationComponent: from NewReservationResolver');
    } else {
      this.selectedInitialDate = rd.serverTime;
      this.freeRoomsDataSource = new CustomDataSource<FreeRoom, FreeRoomFilter>(
        this.apiFreeRooms
      );
      this.freeRoomsDataSource.setData(rd.freeRoomsListAndCount.list, rd.freeRoomsListAndCount.cnt);
      this.agencies = rd.agenciesListAndCount.list;
    }
  }

  onSelectInitialDate(date: Date): void {
    this.selectedInitialDate = date;
    this.selectionFreeRooms.clear();
    this.reloadFreeRooms();
  }

  onSelectionFreeRoomsChange(row): void {
    this.selectionFreeRooms.toggle(row);

    if ( !this.selectionFreeRooms.selected || this.selectionFreeRooms.selected.length === 0 ) {
      this.selectedRoom = '';
    } else {
      this.selectedRoom = this.selectionFreeRooms.selected[0].room.id.toString();

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
    this.selectedRoom = '';
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
}
