import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-add-user',
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
          <form [formGroup]="createForm" #f="ngForm" (ngSubmit)="onCreateMes()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Crear Usuario</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Nombre"
                    formControlName="firstname"
                  />
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Apellidos"
                    formControlName="lastname"
                  />
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Correo"
                    formControlName="email"
                  />
                </mat-form-field>


                <mat-form-field class="full-width">
                  <input matInput required #password type ="password" placeholder="Contraseña" formControlName="password">
                  <mat-hint align="end">6 / {{password.value.length}}</mat-hint>
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="password"
                    placeholder="Repetir contraseña"
                    formControlName="repeat_password"
                  />
                  <mat-hint *ngIf="(createForm.value.password != createForm.value.repeat_password)
                    || (createForm.value.password.length <= 6 && createForm.value.password.length > 1)">
                    <span class="mat-warn">Las contraseñas no coinciden, debe poseer un mínimo de 6 carácteres</span>
                  </mat-hint>
                  <mat-hint *ngIf="(createForm.value.password == createForm.value.repeat_password)
                   && (createForm.value.password.length > 6)">
                    <span class="mat-accent">Las contraseñas coinciden</span>
                  </mat-hint>
                </mat-form-field>



              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!createForm.valid ||
                  (createForm.value.password != createForm.value.repeat_password)" aria-label="create">
                  <mat-icon>add</mat-icon>
                  <span>Usuario</span>
                </button>

                <button mat-raised-button color="accent"
                      routerLink="/admin/user" routerLinkActive type="button" aria-label="list">
                  <mat-icon>list</mat-icon>
                  <span>Listado de usuarios</span>
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
export class AddUserComponent implements OnInit {

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
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      repeat_password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onCreateMes(): void {
    this.loading = true;

    if (this.createForm.valid) {
      this.createForm.disable();

      this.httpClient.post(`${env.serverUrl}/User/Create`, {
        firstname: this.createForm.value.firstname,
        lastname: this.createForm.value.lastname,
        email: this.createForm.value.email,
        password: this.createForm.value.password,
      }).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Usuario con nombre ${this.createForm.value.firstname} ha sido creado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'user']);
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
