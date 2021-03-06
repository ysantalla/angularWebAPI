import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-add-agency',
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
          <form [formGroup]="createForm" #f="ngForm" (ngSubmit)="onCreateAgency()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Crear Agencia</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Nombre de la agencia"
                    formControlName="name"
                  />
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Correo electrónico"
                    formControlName="email"
                  />
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Nombre del representante"
                    formControlName="represent"
                  />
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="number"
                    placeholder="Teléfono"
                    formControlName="phone"
                  />
                </mat-form-field>


              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!createForm.valid" aria-label="create">
                  <mat-icon>add</mat-icon>
                  <span>Agencia</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/agency" routerLinkActive type="button" aria-label="list">
                  <mat-icon>list</mat-icon>
                  <span>Listado de agencias</span>
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
export class AddAgencyComponent implements OnInit {

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
      name: ['', Validators.required],
      represent: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required]
    });
  }

  onCreateAgency(): void {
    this.loading = true;

    if (this.createForm.valid) {
      this.createForm.disable();

      this.httpClient.post(`${env.serverUrl}/agencies`, {
        name: this.createForm.value.name,
        represent: this.createForm.value.represent,
        email: this.createForm.value.email,
        phone: this.createForm.value.phone
      }).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Agencia con nombre ${this.createForm.value.name} ha sido creado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'agency']);
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
