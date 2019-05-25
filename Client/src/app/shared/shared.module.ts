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
  MatChipsModule

} from '@angular/material';

import { RouterModule } from '@angular/router';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { SizePipe } from './pipes/size.pipe';
import { CdkTableModule } from '@angular/cdk/table';
import { CustomMatPaginatorIntl } from './class/mat-paginator-intl';
import { ConfirmComponent } from './components/confirm/confirm.component';

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

    RouterModule
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
    MatDialogModule,
    MatChipsModule,

    LayoutComponent,
    SizePipe,
    ConfirmComponent
  ],
  declarations: [LayoutComponent, NavMenuComponent, SizePipe, ConfirmComponent],
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
