import { Component, OnInit, ViewChild } from '@angular/core';
import { CustomDataSource } from '@app/core/datasources/custom-datasource';
import { FreeRoom, FreeRoomFilter } from '@app/core/models/core';
import { ApiFreeRoomService } from '@app/core/services/core';
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

  constructor(private route: ActivatedRoute,
              private apiFreeRooms: ApiFreeRoomService) { }

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
      this.freeRoomsDataSource.setData(rd.freeRooms, rd.freeRoomsCnt);
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
      console.log('selectedRoom =', this.selectedRoom);
      setTimeout(
        () => {this.stepper.next();},
        1000
      );
    }
  }

  reloadFreeRooms(): void {
    this.selectionFreeRooms = new SelectionModel<FreeRoom>(false, []);
    this.selectedRoom = '';
    this.freeRoomsDataSource.load( new FreeRoomFilter(this.selectedInitialDate) );
  }
}
