import {HttpClient, HttpParams, HttpErrorResponse} from '@angular/common/http';
import {Component, ViewChild, AfterViewInit, OnDestroy, OnInit} from '@angular/core';
import {MatPaginator, MatSort, MatSnackBar, MatDialog} from '@angular/material';
import {merge, of as observableOf, Subject, Subscription} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';

import { environment as env } from '@env/environment';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ConfirmComponent } from '@app/shared/components/confirm/confirm.component';

@Component({
  selector: 'app-list-country',
  template: `
    <div *ngIf="loading">
      <mat-progress-bar color="warn"></mat-progress-bar>
    </div>

    <form
          [formGroup]="searchForm"
          #f="ngForm"
          (ngSubmit)="onFilter()"
          class="form">

      <div class="container padding-15">

        <div class="item-70 container">

          <div class="item-40">
            <mat-form-field class="full-width">
              <input
                matInput
                type="text"
                placeholder="Filtrado por nombre de país"
                formControlName="name"
              />
            </mat-form-field>
          </div>

          <div class="item-25" style="margin-top: 10px;">

            <button
              mat-raised-button
              color="primary"
              type="submit"
              [disabled]="!searchForm.valid"
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
              [disabled]="searchForm.valid || searchForm.value.name === ''"
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
            routerLink="/admin/country/add"
            aria-label="add"
          >
            <mat-icon>add</mat-icon>
            <span> País </span>
          </button>


        </div>

      </div>

    </form>

    <div *ngIf="data.length === 0" class="container">
      <div class="item">
        <h1 class="mat-h1">No hay items</h1>
      </div>
    </div>

    <div [hidden]="data.length === 0" class="table mat-elevation-z8">
      <div class="loading-shade" *ngIf="loading"></div>

      <div class="table-container">

        <table mat-table [dataSource]="data" class="table"
              matSort matSortActive="name" matSortDisableClear matSortDirection="asc">

          <!-- Id Column -->
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef mat-sort-header disableClear>
              Id
            </th>
            <td mat-cell *matCellDef="let row">{{row.id}}</td>
          </ng-container>

          <!-- Name Column -->
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header disableClear>
              Nombre
            </th>
            <td mat-cell *matCellDef="let row">{{row.name}}</td>
          </ng-container>

          <!-- edit Column -->
          <ng-container matColumnDef="edit">
            <th mat-header-cell *matHeaderCellDef>
              Editar
            </th>
            <td mat-cell *matCellDef="let row">
              <a mat-button color="accent" [routerLink]="['/admin','country', 'edit', row.id]">
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

      <mat-paginator [length]="resultsLength" [pageSize]="20"></mat-paginator>
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
export class ListCountryComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['id', 'name', 'edit', 'delete'];

  data: any[] = [];

  searchForm: FormGroup;
  load$ = new Subject<string | null>();

  resultsLength = 0;
  loading = true;

  preloading = false;

  subscription: Subscription;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
      private httpClient: HttpClient,
      private formBuilder: FormBuilder,
      private activatedRoute: ActivatedRoute,
      private snackBar: MatSnackBar,
      private dialog: MatDialog
    ) {
    this.searchForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    this.subscription = merge(this.sort.sortChange, this.paginator.page, this.load$)
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
              .set('filter.searchString', this.searchForm.value.name || '')
              .set('paginator.offset', (this.paginator.pageIndex * this.paginator.pageSize).toString())
              .set('paginator.limit', this.paginator.pageSize.toString())
              .set('orderBy.by', this.sort.active)
              .set('orderBy.desc', (this.sort.direction === 'desc').toString());

              return this.httpClient.get<any>(`${env.serverUrl}/countries`, {params: params});
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
    if (this.searchForm.valid) {
      this.load$.next(this.searchForm.value.name);
      this.searchForm.disable();
    }
  }

  onClear(): void {
    this.searchForm.reset();
    this.searchForm.enable();
    this.load$.next('');
  }

  onDelete(item: any): void {
    const dialogRef = this.dialog.open(ConfirmComponent, {
      data: {
        message: `¿Está seguro que desea eliminar el país "${
          item.name
        }"?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loading = true;
        this.httpClient.delete(`${env.serverUrl}/countries/${item.id}`).subscribe(data => {

          this.load$.next('');

        });
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
