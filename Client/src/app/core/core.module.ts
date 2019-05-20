import {
  NgModule,
  SkipSelf,
  Optional,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  HttpClientModule, HTTP_INTERCEPTORS,
} from '@angular/common/http';

import { AuthGuard } from '@app/core/guards/auth.guard';
import { RoleGuard } from '@app/core/guards/role.guard';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LocalStorageService } from './services/local-storage.service';
import { AuthService } from './services/auth.service';


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
    }
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
