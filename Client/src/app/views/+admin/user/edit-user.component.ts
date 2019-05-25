import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-edit-user',
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
          <form [formGroup]="editForm" #f="ngForm" (ngSubmit)="onEdit()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Editar Usuario</h1>
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
                  <input matInput #password type ="password" placeholder="Contraseña" formControlName="password">
                  <mat-hint align="end">6 / {{password.value.length}}</mat-hint>
                </mat-form-field>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    type="password"
                    placeholder="Repetir contraseña"
                    formControlName="repeat_password"
                  />
                  <mat-hint *ngIf="(editForm.value.password != editForm.value.repeat_password)
                    || (editForm.value.password.length <= 6 && editForm.value.password.length > 1)">
                    <span class="mat-warn">Las contraseñas no coinciden, debe poseer un mínimo de 6 carácteres</span>
                  </mat-hint>
                  <mat-hint *ngIf="(editForm.value.password == editForm.value.repeat_password)
                  && (editForm.value.password.length > 6)">
                    <span class="mat-accent">Las contraseñas coinciden</span>
                  </mat-hint>
                </mat-form-field>


              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!editForm.valid" aria-label="edit">
                  <mat-icon>mode_edit</mat-icon>
                  <span>Usuario</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/user" routerLinkActive type="button" aria-label="list">
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
export class EditUserComponent implements OnInit {

  editForm: FormGroup;
  loading = false;

  itemId: string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private snackBar: MatSnackBar,
    private httpClient: HttpClient
  ) { }

  ngOnInit() {

    this.editForm = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      repeat_password: ['']
    });

    this.itemId = this.activatedRoute.snapshot.params['id'];

    this.loading = true;
    const params = new HttpParams()
      .set('Id', this.itemId);

    this.httpClient.get(`${env.serverUrl}/User/GetById`, {params: params}).subscribe((data: any) => {
      this.loading = false;

      this.editForm.patchValue({
        firstname: data.firstname,
        lastname: data.lastname,
        email: data.email
      });

    });

  }

  onEdit(): void {
    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();

      const editValues = {
        Id: this.itemId,
        firstname: this.editForm.value.firstname,
        lastname: this.editForm.value.lastname,
        email: this.editForm.value.email,
        password: this.editForm.value.password
      };

      this.httpClient.patch(`${env.serverUrl}/User/Update`, editValues).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`User con nombre ${this.editForm.value.firstname} ha sido editado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'user']);
        }
        this.loading = false;

      }, (error: HttpErrorResponse) => {
        this.loading = false;
        this.editForm.enable();
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });

    } else {
      console.log('Form not valid');
    }
  }
}
