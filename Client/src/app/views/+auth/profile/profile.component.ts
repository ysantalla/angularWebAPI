import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import {environment as env} from '@env/environment';
import { User } from '@app/core/models/user.model';


@Component({
  selector: 'app-profile',
  template: `
    <div class="loading">
      <mat-progress-bar *ngIf="loading" color="warn"></mat-progress-bar>
    </div>
    <br />
    <div class="container">
      <div class="item">

        <mat-card class="card">
            <mat-toolbar color="primary">
              <mat-card-header>
                <h1 class="mat-h1">Perfil de Usuario</h1>
            </mat-card-header>
          </mat-toolbar>

          <br />

          <mat-card-content>

            <mat-grid-list *ngIf="userProfile" cols="2" rowHeight="120">
              <mat-grid-tile><h3 class="mat-h3">Nombre:</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3"> {{userProfile.firstname}}</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3">Apellidos:</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3"> {{userProfile.lastname}}</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3">Correo:</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3"> {{userProfile.email}}</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3">Usuario:</h3></mat-grid-tile>
              <mat-grid-tile><h3 class="mat-h3"> {{userProfile.userName}}</h3></mat-grid-tile>
            </mat-grid-list>

          </mat-card-content>

          <br />
          <mat-card-actions>
            <button mat-raised-button color="accent" type="button"
              routerLink="/dashboard" routerLinkActive="active"
              aria-label="dashboard">
            <mat-icon>dashboard</mat-icon>
            <span>Escritorio</span>
          </button>
          </mat-card-actions>
        </mat-card>

      </div>
    </div>
    `,
    styles: [`
      .card {
        width: 40vw;
      }
    `]
})
export class ProfileComponent implements OnInit {

  loading = false;

  userProfile: User;

  constructor(
    private snackBar: MatSnackBar,
    private httpClient: HttpClient
  ) { }

  ngOnInit() {

    this.loading = true;

    this.httpClient.get(`${env.serverUrl}/Account/Profile`).subscribe((data: any) => {
      this.loading = false;
      if (data) {
        this.userProfile = data;
      }

    }, (error: HttpErrorResponse) => {
      this.snackBar.open(error.error, 'X', {duration: 3000});
    });

  }

}
