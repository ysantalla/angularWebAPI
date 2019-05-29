import {HttpClient, HttpParams, HttpErrorResponse} from '@angular/common/http';
import {Component, ViewChild, AfterViewInit, OnDestroy, OnInit} from '@angular/core';
import {MatPaginator, MatSort, MatSnackBar, MatDialog} from '@angular/material';
import {merge, of as observableOf, Subject, Subscription} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';

import { environment as env } from '@env/environment';
import { ActivatedRoute } from '@angular/router';
import { ConfirmComponent } from '@app/shared/components/confirm/confirm.component';
import { GuestSelectorComponent } from '@app/shared/components/guest-selector/guest-selector.component';

@Component({
  selector: 'app-list-reservation',
  template: `
    <div *ngIf="loading">
      <mat-progress-bar color="warn"></mat-progress-bar>
    </div>

    <div style="padding: 10px;">
      <div class="container padding-15">
        <div class="item-70 container">
          <div class="item-40">
            <app-guest-selector></app-guest-selector>
          </div>

          <div class="item-25" style="margin-top: 10px;">

            <button
              mat-raised-button
              color="primary"
              [disabled]="(!guestSelector.ValidSelection() || disableGuestSelector)"
              aria-label="search"
            >
              <mat-icon>search</mat-icon>
              <span>Filtrar</span>
            </button>



            <button
              mat-raised-button
              color="accent"
              type="button"
              (click)="onClear()"
              aria-label="clear"
              [disabled]="guestSelector.ValidSelection()"
            >
              <mat-icon>clear</mat-icon>
              <span>Limpiar</span>
            </button>
          </div>

        </div>


        <div class="item-30" style="margin-top: 10px;">
          <button
            mat-raised-button
            color="primary"
            type="button"
            routerLink="/admin/reservation/add"
            aria-label="add"
          >
            <mat-icon>add</mat-icon>
            <span> Reservación </span>
          </button>


        </div>

      </div>
    </div>

    <div *ngIf="data.length === 0" class="container">
      <div class="item">
        <h1 class="mat-h1">No hay resultados</h1>
      </div>
    </div>

    <div [hidden]="data.length === 0" class="table mat-elevation-z8">
      <div class="loading-shade" *ngIf="loading"></div>

      <div class="table-container">

        <table mat-table [dataSource]="data" class="table">

          <!-- InitDate Column -->
          <ng-container matColumnDef="initDate">
            <th mat-header-cell *matHeaderCellDef>
              Inicio
            </th>
            <td mat-cell *matCellDef="let row">{{row.initDate}}</td>
          </ng-container>

          <!-- EndDate Column -->
          <ng-container matColumnDef="endDate">
            <th mat-header-cell *matHeaderCellDef>
              Fin
            </th>
            <td mat-cell *matCellDef="let row">{{row.endDate}}</td>
          </ng-container>

          <!-- Guest Column -->
          <ng-container matColumnDef="guest">
            <th mat-header-cell *matHeaderCellDef>
              Huésped
            </th>
            <td mat-cell *matCellDef="let row">{{row.guest.name}}</td>
          </ng-container>

          <!-- Room Column -->
          <ng-container matColumnDef="room">
            <th mat-header-cell *matHeaderCellDef>
              Cuarto
            </th>
            <td mat-cell *matCellDef="let row">{{row.room.number}}</td>
          </ng-container>

          <!-- Package Column -->
          <ng-container matColumnDef="package">
            <th mat-header-cell *matHeaderCellDef>
              Paquete
            </th>
            <td mat-cell *matCellDef="let row">{{row.package.name}}</td>
          </ng-container>

          <!-- edit Column -->
          <ng-container matColumnDef="edit">
            <th mat-header-cell *matHeaderCellDef>
              Editar
            </th>
            <td mat-cell *matCellDef="let row">
              <a mat-button color="accent" [routerLink]="['/admin','reservation', 'edit', row.id]">
                <mat-icon>edit</mat-icon>
              </a>
            </td>
          </ng-container>

          <!-- delete Column -->
          <ng-container matColumnDef="delete">
            <th mat-header-cell *matHeaderCellDef>
              Eliminar
            </th>
            <td mat-cell *matCellDef="let row">
              <a mat-button (click)="onDelete(row)" color="warn"><mat-icon>delete_forever</mat-icon></a>
            </td>
          </ng-container>


          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </div>

      <mat-paginator [length]="resultsLength" [pageSize]="2"></mat-paginator>
    </div>

  `,
  styles: [`
    /* Structure */
    .table-container {
      position: relative;
    }

    button {
      margin-left: 4px;
    }

    .padding-15 {
      padding: 15px;
    }

    .item-25 {
      width: 25vw;
    }

    .item-30 {
      width: 30vw;
    }

    .item-70 {
      width: 70vw;
    }

    .item-40 {
      width: 40vw;
    }

    table {
      width: 100%;
    }

    .loading-shade {
      position: absolute;
      top: 0;
      left: 0;
      bottom: 56px;
      right: 0;
      background: rgba(0, 0, 0, 0.15);
      z-index: 1;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    /* Column Widths */
    .mat-column-id,
    .mat-column-edit {
      max-width: 20px;
    }

    .mat-column-name {
      max-width: 100%;
    }

    .full-width {
      width: 100%;
    }
  `]
})
export class ListReservationComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['initDate', 'endDate' , 'guest' , 'room', 'package' , 'edit', 'delete'];

  data: any[] = [];

  load$ = new Subject<number | null>();

  resultsLength = 0;
  loading = true;

  preloading = false;

  subscription: Subscription;

  disableGuestSelector = false;
  @ViewChild(GuestSelectorComponent) guestSelector: GuestSelectorComponent;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
      private httpClient: HttpClient,
      private activatedRoute: ActivatedRoute,
      private snackBar: MatSnackBar,
      private dialog: MatDialog
    ) {
  }

  ngOnInit(): void {

    this.subscription = merge(this.paginator.page, this.load$)
        .pipe(
          startWith({}),
          switchMap(() => {
            this.loading = true;

            if (!this.preloading) {
              this.preloading = true;
              return this.activatedRoute.data.pipe(
                map((resolve: any) => resolve.data)
              );

            } else {
              const params = new HttpParams()
              .set('filter.guestID',
                  (this.guestSelector.ValidSelection() ? this.guestSelector.autoGuestSelection.ID : 0).toString())
              .set('paginator.offset', (this.paginator.pageIndex * this.paginator.pageSize).toString())
              .set('paginator.limit', this.paginator.pageSize.toString());

              return this.httpClient.get<any>(`${env.serverUrl}/reservations`, {params: params});
            }
          }),
          map(data => {
            // Flip flag to show that loading has finished.
            this.loading = false;
            this.resultsLength = data.countItems;

            return data.value;
          }),
          catchError(() => {
            this.loading = false;
            return observableOf([]);
          })
        ).subscribe((data: any) => {
            this.data = data;
        }, (error: HttpErrorResponse) => {
          this.snackBar.open(error.error, 'X', {duration: 3000});
        });
  }

  onFilter(): void {
    if (this.guestSelector.ValidSelection()) {
      this.load$.next(this.guestSelector.autoGuestSelection.ID);
      this.disableGuestSelector = true;
    }
  }

  onClear(): void {
    this.guestSelector.Clear();
    this.disableGuestSelector = false;
    this.load$.next(0);
  }

  onDelete(item: any): void {
    const dialogRef = this.dialog.open(ConfirmComponent, {
      data: {
        message: `¿Está seguro que desea eliminar la reservación "${
          item.name
        }"?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loading = true;
        this.httpClient.delete(`${env.serverUrl}/reservations/${item.id}`).subscribe(data => {
          this.load$.next(0);
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
