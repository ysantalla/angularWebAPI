import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment as env } from '@env/environment';
import { Agency, Guest, Room } from '@app/core/models/core';


@Component({
  selector: 'app-add-reservation',
  template: `
    <div class="container">
      <div class="loading">
        <mat-progress-bar value="indeterminate" *ngIf="loading" color="warn"></mat-progress-bar>
      </div>
    </div>
    <br />
    <div class="container">
      <div class="item">

        <div class="mat-elevation-z8">
          <form [formGroup]="createForm" #f="ngForm" (ngSubmit)="onCreate()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Crear País</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Nombre de país"
                    formControlName="name"
                  />
                </mat-form-field>

              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!createForm.valid" aria-label="create">
                  <mat-icon>add</mat-icon>
                  <span>País</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/reservations" routerLinkActive type="button" aria-label="list">
                  <mat-icon>list</mat-icon>
                  <span>Listado de países</span>
                </button>
              </mat-card-actions>
            </mat-card>
          </form>
        </div>
      </div>
    </div>

  `,
  styles: [`
    .full-width {
      width: 100%;
    }
  `]
})
export class AddReservationComponent implements OnInit {

  createForm: FormGroup;
  loading = false;

  agencies: Agency[] = [];
  guests: Guest[] = [];
  rooms: Room[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private httpClient: HttpClient
  ) { }

  ngOnInit() {
    const rd = this.route.snapshot.data.resolverData;
    console.log('data = ', rd);

    this.agencies = rd.agencies;
    this.guests = rd.guests;
    this.rooms = rd.rooms;

    console.log('agencies = ', this.agencies);
    console.log('guests = ', this.guests);
    console.log('rooms = ', this.rooms);

    this.createForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  onCreate(): void {
    console.log('onCreate');
  }
}
