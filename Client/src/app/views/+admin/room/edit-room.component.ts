import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-edit-room',
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
                  <h1 class="mat-h1">Editar Cuarto</h1>
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

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="checkbox"
                    placeholder="Disponible"
                    formControlName="enable"
                  />
                </mat-form-field>

              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!editForm.valid" aria-label="editRoom">
                  <mat-icon>mode_edit</mat-icon>
                  <span>Cuarto</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/rooms" routerLinkActive type="button" aria-label="backToList">
                  <mat-icon>list</mat-icon>
                  <span>Listado de cuartos</span>
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
export class EditRoomComponent implements OnInit {

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
      number: ['', Validators.required],
      description: ['', Validators.required],
      capacity: [1, Validators.required, Validators.min(1)],
      enable: [false, Validators.required],
      bedCont: [1, Validators.required, Validators.min(1)]
    });

    this.itemId = this.activatedRoute.snapshot.params['id'];

    this.loading = true;

    this.httpClient.get(`${env.serverUrl}/rooms/${this.itemId}`).subscribe((data: any) => {
      this.loading = false;

      this.editForm.patchValue({
        number: data.value.number,
        description: data.value.description,
        capacity: data.value.capacity,
        enable: data.value.enable,
        bedCont: data.value.bedCont
      });

    });

  }

  onEdit(): void {
    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();

      this.httpClient.patch(`${env.serverUrl}/rooms/${this.itemId}`, {
        id: this.itemId,
        number: this.editForm.value.number,
        description: this.editForm.value.description,
        capacity: this.editForm.value.capacity,
        enable: this.editForm.value.enable,
        bedCont: this.editForm.value.enable,
      }).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Cuarto con número ${this.editForm.value.name} ha sido editado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'rooms']);
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
