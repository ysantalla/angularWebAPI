import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-add-room',
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
          <form [formGroup]="createForm" #f="ngForm" (ngSubmit)="onCreateRoom()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Crear Cuarto</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

              <mat-form-field class="full-width">
                <input
                  matInput
                  required
                  type="text"
                  placeholder="Número"
                  formControlName="number"
                />
              </mat-form-field>

              <mat-form-field class="full-width">
                <input
                  matInput
                  required
                  type="text"
                  placeholder="Descripción"
                  formControlName="description"
                />
              </mat-form-field>

              <mat-form-field class="full-width">
                <input
                  matInput
                  required
                  min="1"
                  type="number"
                  placeholder="Capacidad"
                  formControlName="capacity"
                />
              </mat-form-field>

              <mat-form-field class="full-width">
                <input
                  matInput
                  required
                  min="1"
                  type="number"
                  placeholder="Camas"
                  formControlName="bedCont"
                />
              </mat-form-field>

              <mat-checkbox class="full-width"
                formControlName="enable">
                Disponible
              </mat-checkbox>


              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!createForm.valid" aria-label="create">
                  <mat-icon>add</mat-icon>
                  <span>Cuarto</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/rooms" routerLinkActive type="button" aria-label="backToList">
                  <mat-icon>list</mat-icon>
                  <span>Listado de Cuartos</span>
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
export class AddRoomComponent implements OnInit {

  createForm: FormGroup;
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private httpClient: HttpClient
  ) { }

  ngOnInit() {

    this.createForm = this.formBuilder.group({
      number: ['', Validators.required],
      description: ['', Validators.required],
      enable: [false, Validators.required],
      capacity: [1, [Validators.required, Validators.min(1)]],
      bedCont: [1, [Validators.required, Validators.min(1)]]
    });
  }

  onCreateRoom(): void {
    this.loading = true;

    if (this.createForm.valid) {
      this.createForm.disable();

      this.httpClient.post(`${env.serverUrl}/rooms`, {
        number: this.createForm.value.number,
        description: this.createForm.value.description,
        enable: this.createForm.value.enable,
        capacity: this.createForm.value.capacity,
        bedCont: this.createForm.value.bedCont
      }).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Cuarto con número ${this.createForm.value.number} ha sido creado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'rooms']);
        }
        this.loading = false;

      }, (error: HttpErrorResponse) => {
        this.loading = false;
        this.createForm.enable();
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });

    } else {
      console.log('Form not valid');
    }
  }
}
