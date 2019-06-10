import { EmigrationReservationResolver } from './emigration-report/emigration-reservation.resolver';
import { EmigrationReportComponent } from './emigration-report/emigration-report.component';
import { ReportReservationResolver } from './reservation-report/report-reservation.resolver';

import { ReservationReportComponent } from './reservation-report/reservation-report.component';
import { Routes } from '@angular/router';
import { ReservationsComponent } from './reservations/reservations.component';
import { CheckInComponent } from './check-in/check-in.component';
import { CheckOutComponent } from './check-out/check-out.component';
import { CheckInResolver } from './check-in/check-in.resolver';
import { NewReservationComponent } from './new-reservation/new-reservation.component';
import { NewReservationResolver } from './new-reservation/new-reservation.resolver';
import { CheckOutResolver } from './check-out/check-out.resolver';
import { RoleGuard } from '@app/core/guards/core';
import { ReservationsResolver } from './reservations/reservations.resolver';
import { InvoiceResolver } from './invoice/invoice.resolver';
import { InvoiceComponent } from './invoice/invoice.component';

export const ReceptionRoutes: Routes = [
  {
    path: '',
    redirectTo: 'reservations',
    pathMatch: 'full',
  },
  {
    path: 'check-in',
    component: CheckInComponent,
    canActivate: [RoleGuard],
    data: { title: 'Check-In', expectedRole: ['Manager']},
    resolve: {
      CheckInPageData: CheckInResolver,
    }
  },
  {
    path: 'check-out',
    component: CheckOutComponent,
    canActivate: [RoleGuard],
    data: { title: 'Check-Out', expectedRole: ['Manager']},
    resolve: {
      CheckOutPageData: CheckOutResolver,
    }
  },
  {
    path: 'new-reservation',
    component: NewReservationComponent,
    canActivate: [RoleGuard],
    data: { title: 'Nueva Reservación', expectedRole: ['Manager']},
    resolve: {
      NewReservationPageData: NewReservationResolver,
    }
  },
  {
    path: 'reservations',
    component: ReservationsComponent,
    canActivate: [RoleGuard],
    data: { title: 'Reservaciones', expectedRole: ['Manager']},
    resolve: {
      ReservationsResolver: ReservationsResolver
    }
  },
  {
    path: 'reservation-report',
    component: ReservationReportComponent,
    canActivate: [RoleGuard],
    data: { title: 'Reporte de Reservaciones', expectedRole: ['Manager']},
    resolve: {
      reportReservation: ReportReservationResolver,
    }
  },
  {
    path: 'emigration-report',
    component: EmigrationReportComponent,
    canActivate: [RoleGuard],
    data: { title: 'Reporte de Emigración', expectedRole: ['Manager']},
    resolve: {
      reportReservation: EmigrationReservationResolver,
    }
  },
  {
    path: 'invoice',
    component: InvoiceComponent,
    canActivate: [RoleGuard],
    data: { title: 'Reporte Facturas', expectedRole: ['Manager']},
    resolve: {
      invoice: InvoiceResolver,
    }
  }
];
