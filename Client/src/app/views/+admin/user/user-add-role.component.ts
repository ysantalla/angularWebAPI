import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment as env } from '@env/environment';


@Component({
  selector: 'app-user-add-role',
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
          <form [formGroup]="selectForm" #f="ngForm" (ngSubmit)="onRoleAdd()" class="form">
            <mat-card class="mes-card">
              <mat-toolbar>
                <mat-card-header>
                  <h1 class="mat-h1">Seleccionar Rol</h1>
                </mat-card-header>
              </mat-toolbar>

              <br />

              <mat-card-content>

                <mat-form-field class="full-width">
                  <mat-select placeholder="Escoja un rol" multiple formControlName="roles" required>
                    <mat-option *ngFor="let role of roles | async" [value]="role.name">{{role.name}}</mat-option>
                  </mat-select>
                </mat-form-field>

              </mat-card-content>
              <mat-card-actions>

                <button mat-raised-button color="primary" type="submit" [disabled]="!selectForm.valid" aria-label="create">
                  <mat-icon>add</mat-icon>
                  <span>Adicionar Rol</span>
                </button>

                <button mat-raised-button color="accent" routerLink="/admin/user" routerLinkActive type="button" aria-label="list">
                  <mat-icon>list</mat-icon>
                  <span>Listado de Usuarios</span>
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
export class UserAddRoleComponent implements OnInit {

  selectForm: FormGroup;
  loading = false;

  roles: Observable<any[]>;

  itemId: string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private snackBar: MatSnackBar,
    private httpClient: HttpClient
  ) { }

  ngOnInit() {

    this.selectForm = this.formBuilder.group({
      roles: ['', Validators.required]
    });

    this.roles = this.httpClient.get<any>(`${env.serverUrl}/Role/List`).pipe(
      map(data => {
        return data.value;
      })
    );

    this.itemId = this.activatedRoute.snapshot.params['id'];

    this.httpClient.get<any>(`${env.serverUrl}/User/GetRolesByUser?id=${this.itemId}`).subscribe((data: any) => {
      if (data) {
        this.selectForm.patchValue({
          roles: data
        });
      }
    });
  }

  onRoleAdd(): void {
    this.loading = true;

    if (this.selectForm.valid) {
      this.selectForm.disable();

      this.httpClient.post(`${env.serverUrl}/User/AddRole`,
                {userId: this.itemId, roles: this.selectForm.value.roles}).subscribe((data: any) => {

        if (data.succeeded) {
          this.snackBar.open(`Seleccionando nuevos roles`, 'X', {duration: 3000});
          this.router.navigate(['admin', 'user']);
        }
        this.loading = false;

      }, (error: HttpErrorResponse) => {
        this.loading = false;
        this.selectForm.enable();
        this.snackBar.open(error.error, 'X', {duration: 3000});
      });

    } else {
      console.log('Form not valid');
    }
  }
}
