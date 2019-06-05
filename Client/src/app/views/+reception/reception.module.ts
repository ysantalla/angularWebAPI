import { EditCheckInComponent } from './check-in/edit-checkin.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReservationsComponent } from './reservations/reservations.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { RouterModule } from '@angular/router';

import { ReceptionRoutes } from './reception.routing';
import { CheckInComponent } from './check-in/check-in.component';
import { CheckOutComponent } from './check-out/check-out.component';
import { CheckInResolver } from './check-in/check-in.resolver';
import { ReservationDialogComponent } from './reservation-dialog/reservation-dialog.component';
import { NewReservationComponent } from './new-reservation/new-reservation.component';
import { NewReservationResolver } from './new-reservation/new-reservation.resolver';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(ReceptionRoutes)
  ],
  declarations: [
    ReservationsComponent,
    CheckInComponent,
    CheckOutComponent,
    ReservationDialogComponent,
    NewReservationComponent,
    EditCheckInComponent
  ],
  providers: [
    CheckInResolver,
    NewReservationResolver,
  ],
  entryComponents: [
    EditCheckInComponent
  ],
})
export class ReceptionModule { }
