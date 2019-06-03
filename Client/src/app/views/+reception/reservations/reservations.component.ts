import { Component, OnInit } from '@angular/core';
import { ApiReservationService } from '@app/core/services/core';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styles: []
})
export class ReservationsComponent implements OnInit {



  constructor(private apiReservation: ApiReservationService) {

  }

  ngOnInit() {
  }

}
