import { CoreModule, provideAbpCore, withOptions } from '@abp/ng.core';
import { registerLocale } from '@abp/ng.core/locale';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideSettingManagementConfig } from '@abp/ng.setting-management/config';
import { provideAccountConfig } from '@abp/ng.account/config';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';
import { AccountLayoutModule } from '@abp/ng.theme.lepton-x/account';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { TitleStrategy } from '@angular/router';
import { AppTitleStrategy } from './shared/app-title.strategy';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { SharedModule } from './shared/shared.module';
import { ThemeSharedModule, withHttpErrorConfig, withValidationBluePrint, provideAbpThemeShared } from '@abp/ng.theme.shared';
import { IDENTITY_CONFIG_PROVIDERS } from './shared/identity-config';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule,
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
    AccountLayoutModule.forRoot(),
    ThemeSharedModule,
    SharedModule,
  ],
  declarations: [AppComponent, AccessDeniedComponent, MyProfileComponent],
  providers: [
    APP_ROUTE_PROVIDER,
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale(),
        localizations: [
          {
            culture: 'en',
            resources: [
              {
                resourceName: 'AbpUiNavigation',
                texts: {
                  'Menu:Administration': 'Administration',
                  'Administration': 'Administration'
                }
              }
            ]
          }
        ]
      })
    ),
    provideAbpOAuth(),
    // provideSettingManagementConfig(), // Removed to hide Settings menu
    provideAccountConfig(),
    provideAbpThemeShared(
      withValidationBluePrint({
        wrongPassword: 'Please choose 1q2w3E*',
      })
    ),
    { provide: TitleStrategy, useClass: AppTitleStrategy },
    ...IDENTITY_CONFIG_PROVIDERS
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
