import { EmigrationReservationResolver } from './emigration-report/emigration-reservation.resolver';
import { EmigrationReportComponent } from './emigration-report/emigration-report.component';
import { CheckOutResolver } from './check-out/check-out.resolver';
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
import { NewReservationComponent } from './new-reservation/new-reservation.component';
import { NewReservationResolver } from './new-reservation/new-reservation.resolver';
import { GuestComponent } from './guest/guest.component';
import { EditCheckOutComponent } from './check-out/edit-checkout.component';
import { ReservationReportComponent } from './reservation-report/reservation-report.component';
import { ReportReservationResolver } from './reservation-report/report-reservation.resolver';
import { ReservationsResolver } from './reservations/reservations.resolver';
import { InvoiceResolver } from './invoice/invoice.resolver';
import { InvoiceComponent } from './invoice/invoice.component';
import { ReservationDialogComponent } from './invoice/reservation-dialog.component';

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
    NewReservationComponent,
    EditCheckInComponent,
    EditCheckOutComponent,
    ReservationDialogComponent,
    GuestComponent,
    ReservationReportComponent,
    EmigrationReportComponent,
    InvoiceComponent
  ],
  providers: [
    CheckInResolver,
    CheckOutResolver,
    NewReservationResolver,
    ReportReservationResolver,
    EmigrationReservationResolver,
    ReservationsResolver,
    InvoiceResolver
  ],
  entryComponents: [
    EditCheckInComponent,
    EditCheckOutComponent,
    ReservationDialogComponent
  ],
})
export class ReceptionModule { }
