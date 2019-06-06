import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from '@app/shared/components/layout/layout.component';
import { LayoutModule } from '@angular/cdk/layout';

import {
  MatToolbarModule,
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatInputModule,
  MatProgressSpinnerModule,
  MatCardModule,
  MatMenuModule,
  MatTooltipModule,
  MatProgressBarModule,
  MatSnackBarModule,
  MatListModule,
  MatExpansionModule,
  MatGridListModule,
  MatTableModule,
  MatPaginatorIntl,
  MatSortModule,
  MatPaginatorModule,
  MatDialogModule,
  MatChipsModule,
  MatSelectModule,
  MatCheckboxModule,
  MatAutocompleteModule,
  MatDividerModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatStepperModule,
  MatSlideToggleModule,
  MatTabsModule,

} from '@angular/material';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { SizePipe } from './pipes/size.pipe';
import { CdkTableModule } from '@angular/cdk/table';
import { CustomMatPaginatorIntl } from './class/mat-paginator-intl';
import { ConfirmComponent } from './components/confirm/confirm.component';
import { GuestSelectorComponent } from './components/guest-selector/guest-selector.component';

@NgModule({
  imports: [
    CommonModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    MatToolbarModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatListModule,
    MatExpansionModule,
    MatGridListModule,
    MatTableModule,
    CdkTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatDividerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatStepperModule,
    MatTabsModule,

    RouterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    MatToolbarModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatSidenavModule,
    MatMenuModule,
    MatIconModule,
    MatTooltipModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatListModule,
    MatExpansionModule,
    MatGridListModule,
    MatTableModule,
    CdkTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDialogModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatDividerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatStepperModule,
    MatSlideToggleModule,
    MatTabsModule,

    LayoutComponent,
    SizePipe,
    ConfirmComponent,
    GuestSelectorComponent,
  ],
  declarations: [
    LayoutComponent,
    NavMenuComponent,
    SizePipe,
    ConfirmComponent,
    GuestSelectorComponent,
  ],
  providers: [
    {provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl}
  ],
  entryComponents: [
    ConfirmComponent
  ],
})
export class SharedModule {
  static forRoot() {
    return {
      ngModule: SharedModule,
      providers: []
    };
  }
}
