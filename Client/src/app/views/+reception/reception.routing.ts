import { Routes } from '@angular/router';
import { ReservationsComponent } from './reservations/reservations.component';
import { AuthGuard } from '@app/core/guards/core';
import { CheckInComponent } from './check-in/check-in.component';
import { CheckOutComponent } from './check-out/check-out.component';
import { ServerTimeResolver } from '../common/server-time.resolver';
import { CheckInResolver } from './check-in/check-in.resolver';
import { NewReservationComponent } from './new-reservation/new-reservation.component';
import { NewReservationResolver } from './new-reservation/new-reservation.resolver';
import { CheckOutResolver } from './check-out/check-out.resolver';

export const ReceptionRoutes: Routes = [
  {
    path: '',
    redirectTo: 'reservations',
    pathMatch: 'full',
  },
  {
    path: 'check-in',
    component: CheckInComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    data: { title: 'Check-In', },
    resolve: {
      CheckInPageData: CheckInResolver,
    }
  },
  {
    path: 'check-out',
    component: CheckOutComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    data: { title: 'Check-Out', },
    resolve: {
      CheckOutPageData: CheckOutResolver,
    }
  },
  {
    path: 'new-reservation',
    component: NewReservationComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    data: { title: 'Nueva Reservación', },
    resolve: {
      NewReservationPageData: NewReservationResolver,
    }
  },
  {
    path: 'reservations',
    component: ReservationsComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    data: { title: 'Reservaciones', },
  },
];
