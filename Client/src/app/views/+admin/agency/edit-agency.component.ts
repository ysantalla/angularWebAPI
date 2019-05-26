import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-edit-agency',
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
                  <h1 class="mat-h1">Editar Agencia</h1>
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
                <button mat-raised-button color="primary" type="submit" [disabled]="!editForm.valid" aria-label="createMes">
                  <mat-icon>mode_edit</mat-icon>
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
export class EditAgencyComponent implements OnInit {

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
      name: ['', Validators.required],
      represent: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required]
    });

    this.itemId = this.activatedRoute.snapshot.params['id'];

    this.loading = true;
    const params = new HttpParams()
      .set('Id', this.itemId);

    this.httpClient.get(`${env.serverUrl}/agencies/${this.itemId}`).subscribe((data: any) => {
      this.loading = false;

      this.editForm.patchValue({
        name: data.value.name,
        email: data.value.email,
        phone: data.value.phone,
        represent: data.value.represent
      });

    });

  }

  onEdit(): void {
    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();

      this.httpClient.patch(`${env.serverUrl}/agencies/${this.itemId}`,
                {
                  name: this.editForm.value.name,
                  email: this.editForm.value.email,
                  phone: this.editForm.value.phone,
                  represent: this.editForm.value.represent,
                }
                ).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Agencia con nombre ${this.editForm.value.name} ha sido editado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'agency']);
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
