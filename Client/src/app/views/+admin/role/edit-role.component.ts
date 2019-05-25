import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-edit-role',
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
                  <h1 class="mat-h1">Editar País</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

                <mat-form-field class="full-width">
                  <input
                    matInput
                    required
                    type="text"
                    placeholder="Nombre del rol"
                    formControlName="name"
                  />
                </mat-form-field>

              </mat-card-content>
              <mat-card-actions>
                <button mat-raised-button color="primary" type="submit" [disabled]="!editForm.valid" aria-label="edit">
                  <mat-icon>mode_edit</mat-icon>
                  <span>Rol</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/role" routerLinkActive type="button" aria-label="list">
                  <mat-icon>list</mat-icon>
                  <span>Listado de roles</span>
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
export class EditRoleComponent implements OnInit {

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
      name: ['', Validators.required]
    });

    this.itemId = this.activatedRoute.snapshot.params['id'];

    this.loading = true;
    const params = new HttpParams()
      .set('Id', this.itemId);

    this.httpClient.get(`${env.serverUrl}/Role/GetById`, {params: params}).subscribe((data: any) => {
      this.loading = false;

      this.editForm.patchValue({
        name: data.name
      });

    });

  }

  onEdit(): void {
    this.loading = true;

    if (this.editForm.valid) {
      this.editForm.disable();

      this.httpClient.patch(`${env.serverUrl}/Role/Update`, {name: this.editForm.value.name, Id: this.itemId}).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Rol con nombre ${this.editForm.value.name} ha sido editado`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'role']);
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
