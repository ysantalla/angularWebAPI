import { Component, OnInit, OnDestroy } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '@app/core/services/auth.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

import { routerTransition } from '@app/core/animations/router.transition';
import { environment as env } from '@env/environment';

import { Menu } from '@app/core/models/menu.model';


@Component({
  selector: 'app-layout',
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav
        #drawer
        class="sidenav"
        fixedInViewport="false"
        [attr.role]="(isHandset$ | async) ? 'dialog' : 'navigation'"
        [mode]="(isHandset$ | async) ? 'over' : 'side'"
        [opened]="!(isHandset$ | async) && (isLoggedIn$ | async)">
        <mat-toolbar class="sidenav-navbar" color="primary">
          <span>{{appName}}</span>
        </mat-toolbar>

        <app-nav-menu [items]="dashboard" *ngIf="isLoggedIn$ | async"></app-nav-menu>

        <app-nav-menu [items]="adminMenu" *ngIf="isAdmin$.asObservable() | async"></app-nav-menu>

        <app-nav-menu [items]="managerMenu" *ngIf="isManager$.asObservable() | async"></app-nav-menu>

        <app-nav-menu [items]="reservations" *ngIf="isManager$.asObservable() | async"></app-nav-menu>

        <app-nav-menu [items]="newReservation" *ngIf="isManager$.asObservable() | async"></app-nav-menu>

        <app-nav-menu [items]="checkIn" *ngIf="isManager$.asObservable() | async"></app-nav-menu>

        <app-nav-menu [items]="checkOut" *ngIf="isManager$.asObservable() | async"></app-nav-menu>

        </mat-sidenav>
      <mat-sidenav-content>
        <mat-toolbar class="navbar" color="primary">
          <mat-toolbar-row>
            <button
              type="button"
              aria-label="Toggle sidenav"
              mat-icon-button
              (click)="drawer.toggle()">
              <mat-icon aria-label="Side nav toggle icon">menu</mat-icon>
            </button>

            <span *ngIf="!(isHandset$ | async)">{{appName}}</span>

            <span class="spacer"></span>

            <button mat-button [matMenuTriggerFor]="menu">
              <span *ngIf="(isLoggedIn$ | async)">Bienvenido {{username$ | async}}</span>
              <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu #menu="matMenu">

              <button mat-menu-item *ngIf="isLoggedIn$ | async" routerLink="auth/profile">
                <mat-icon>person</mat-icon>
                <span>Perfil</span>
              </button>

              <mat-divider *ngIf="isLoggedIn$ | async"></mat-divider>

              <button mat-menu-item *ngIf="isLoggedIn$ | async"  (click)="logout()">
                <mat-icon>exit_to_app</mat-icon>
                <span>Cerrar Sesión</span>
              </button>

              <button mat-menu-item *ngIf="!(isLoggedIn$ | async)" routerLink="auth/login">
                <mat-icon>lock_open</mat-icon>
                <span>Iniciar Sesión</span>
              </button>
            </mat-menu>
          </mat-toolbar-row>
        </mat-toolbar>

        <div class="layout">
          <div class="router" style="background-color: white;">
            <div class="item" [@routerTransition]="o.isActivated && o.activatedRoute.routeConfig.path">
              <router-outlet #o="outlet"></router-outlet>
            </div>
          </div>

          <br/>

          <footer class="footer">
            <mat-toolbar color="primary">
              <span>{{appName}} &#169; {{year}} - Todos los Derechos Reservados</span>
            </mat-toolbar>
          </footer>
        </div>

      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .router {
      min-height: 87vh;
      height: auto;
    }

    .sidenav-container {
      height: 100%;
    }

    mat-nav-list a.menu {
      margin-top: 2px;
      margin-bottom: 2px;
    }

    .navbar {
      box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
      position: sticky;
      z-index: 1025;
      top: 0;
    }

    .sidenav {
      box-shadow: 3px 0 6px rgba(0,0,0,.24);
    }

    .sidenav-navbar {
      box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    }

    .mat-toolbar-row, .mat-toolbar-single-row {
      height: 64px;
    }

    mat-toolbar button.active, mat-toolbar a.active {
      color: white;
      background: rgba(27, 26, 26, 0.2);
      padding-top: 13.5px;
      padding-bottom: 13px;
    }

    mat-toolbar button.mat-button, mat-toolbar a.mat-button {
      color: white;
      padding-top: 13.5px;
      padding-bottom: 13px;
    }

    a.active {
      background: rgba(27, 26, 26, 0.2);
    }

    .logo {
      width: 50px;
      padding-left: 10px;
      padding-right: 10px;
    }

    footer > .mat-toolbar {
      white-space: normal;
      padding-top: 20px;
      height: 80px;
    }

    .mat-list, .mat-nav-list {
      padding-top: 3px;
      padding-bottom: 3px;
      display: block;
    }

  `],
  animations: [routerTransition]
})
export class LayoutComponent implements OnInit, OnDestroy {

  isLoggedIn$: Observable<boolean>;
  username$: Observable<string>;

  public isAdmin$ = new BehaviorSubject(false);
  public isManager$ = new BehaviorSubject(false);

  subscription: Subscription;

  dashboard: Menu;
  adminMenu: Menu;
  managerMenu: Menu;
  reservations: Menu;
  checkIn: Menu;
  checkOut: Menu;
  newReservation: Menu;

  envName = env.envName;
  appName = env.appName;
  year = new Date().getFullYear();
  isProd = env.production;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {


    this.dashboard = {
      heading: 'Escritorio',
      icon: 'dashboard',
      link: '/dashboard',
      pages: []
    };

    this.checkIn = {
      heading: 'Check-In',
      icon: 'room_service',
      link: '/reception/check-in',
      pages: []
    };

    this.checkOut = {
      heading: 'Check-Out',
      icon: 'room_service',
      link: '/reception/check-out',
      pages: []
    };

    this.newReservation = {
      heading: 'Nueva Reservación',
      icon: 'room_service',
      link: '/reception/new-reservation',
      pages: []
    };

    this.reservations = {
      heading: 'Reservaciones',
      icon: 'room_service',
      link: '/reception/reservations',
      pages: []
    };

    this.adminMenu = {
      heading: 'Administración',
      icon: 'settings',
      pages: [
        {
          heading: 'Administrar Usuarios',
          link: '/admin/user/',
          icon: 'person'
        },
        {
          heading: 'Administrar Países',
          link: '/admin/country/',
          icon: 'public'
        },

        {
          heading: 'Administrar Ciudadanía',
          link: '/admin/citizenhips/',
          icon: 'assistant_photo'
        },
        {
          heading: 'Administrar Agencias',
          link: '/admin/agency/',
          icon: 'location_city'
        },
        {
          heading: 'Administrar Cuartos',
          link: '/admin/rooms',
          icon: 'hotel'
        },
        {
          heading: 'Administrar Monedas',
          link: '/admin/currency',
          icon: 'monetization_on'
        }
      ]
    };
    this.managerMenu = {
      heading: 'Recepcionista',
      icon: 'local_activity',
      pages: [
        {
          heading: 'Reporte de Reservación',
          link: '/reception/reservation-report',
          icon: 'list'
        },
        {
          heading: 'Reporte de Emigración',
          link: '/reception/emigration-report',
          icon: 'list'
        },
        {
          heading: 'Reporte Facturas',
          link: '/reception/invoice',
          icon: 'list'
        }
      ]
    };
  }

  ngOnInit(): void {
    this.isLoggedIn$ = this.authService.isAuthenticated();
    this.username$ = this.authService.getUsernameAsync();

    this.subscription = this.authService.getRoleAsync().subscribe( (data: any) => {

        if (data && typeof data === 'object' && data.constructor === Array)  {
          if (data.some(x => x === 'Admin')) {
            this.isAdmin$.next(true);
          }
          if (data.some(x => x === 'Manager')) {
            this.isManager$.next(true);
          }
        } else {
          if (data === 'Admin') {
            this.isAdmin$.next(true);
          }
          if (data === 'Manager') {
            this.isManager$.next(true);
          }
        }
    });

  }

  logout(): void {
    this.isAdmin$.next(false);
    this.isManager$.next(false);
    this.authService.logout();
    this.snackBar.open('Usted a cerrado su sesión', 'X', {duration: 3000});
    this.router.navigate(['auth', 'login']);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
