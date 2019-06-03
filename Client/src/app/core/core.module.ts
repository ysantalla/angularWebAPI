import {
  NgModule,
  SkipSelf,
  Optional,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  HttpClientModule, HTTP_INTERCEPTORS, HttpClient,
} from '@angular/common/http';

import { AuthGuard, RoleGuard } from '@app/core/guards/core';
import { ApiInterceptor } from './interceptors/api.interceptor';
import {
  LocalStorageService,
  AuthService,
  ApiReservationService,
  ApiAgencyService,
  ApiGuestService,
  ApiRoomService,
  ApiFreeRoomService,
} from './services/core';


@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
  ],
  providers: [
    AuthGuard,
    RoleGuard,
    LocalStorageService,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true
    },
    ApiAgencyService,
    ApiGuestService,
    ApiReservationService,
    ApiRoomService,
    ApiFreeRoomService,
  ],
  declarations: []
})
export class CoreModule {

  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule,
  ) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import only in AppModule');
    }
  }
}
