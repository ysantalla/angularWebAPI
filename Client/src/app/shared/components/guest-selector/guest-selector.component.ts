import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MatAutocompleteSelectedEvent, MatSnackBar } from '@angular/material';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

import { environment as env } from '@env/environment';

@Component({
  selector: 'app-guest-selector',
  template: `
<form [formGroup]="form">
  <mat-form-field style="width: 100%;">
    <input  type="text"
            placeholder="Seleccionar huésped"
            matInput
            formControlName="guestname"
            [matAutocomplete]="auto"
            >
    <mat-autocomplete #auto="matAutocomplete"
                      [displayWith]="displayGuest"
                      (optionSelected)="onSelectGuest($event)">
      <mat-option *ngFor="let option of guests"
                  [value]="option">
        {{option.name}}
      </mat-option>
    </mat-autocomplete>
  </mat-form-field>
</form>
  `
})
export class GuestSelectorComponent implements OnInit, OnDestroy {

  form: FormGroup;
  guestnameControl: FormControl;

  guests: any[] = [];
  autoGuestSelection = { ID: 0, Name: '' };
  subscription: Subscription;

  constructor(private httpClient: HttpClient,
              private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.guestnameControl = new FormControl('');
    this.form = new FormGroup({
      'guestname': this.guestnameControl,
    });

    this.guestnameControl.valueChanges
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
      )
      .subscribe(
        (value) => {
          if (value !== '') {
            this.loadGuests(value);
          } else {
            this.guests = [];
          }
        }
      );
  }

  ValidSelection(): boolean {
    return (this.autoGuestSelection.Name !== '' &&
            this.autoGuestSelection === this.guestnameControl.value);
  }

  Clear(): void {
    this.guestnameControl.setValue('');
  }

  loadGuests(value: string): void {
    const params = new HttpParams()
    .set('filter.searchString', this.guestnameControl.value || '')
    .set('paginator.offset', '0')
    .set('paginator.limit', '10')
    .set('orderBy.by', 'name')
    .set('orderBy.desc', 'false');

    this.subscription = this.httpClient.get<any>(`${env.serverUrl}/guests`, {params: params}).subscribe(
      (data: any) => {
        this.guests = data.value;
      }, (error: HttpErrorResponse) => {
        this.snackBar.open(error.error, 'X', {duration: 3000});
      }
    );
  }

  onSelectGuest(ev: MatAutocompleteSelectedEvent) {
    this.autoGuestSelection = ev.option.value;
  }

  displayGuest(guest?: any): string | undefined {
    return guest ? guest.name : undefined;
  }

  ngOnDestroy(): void {
    if ( this.subscription ) {
      this.subscription.unsubscribe();
    }
  }
}
