
import { NgModule, Injectable, Injector, APP_INITIALIZER } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';

import { AppModuleShared } from './app.shared.module';

import { AppComponent } from './components/app/app.component';

import { AppConfig } from './interfaces/home/app-config.module';

import { AppConfigService } from './services/appConfig.service';

@Injectable()
@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        { provide: APP_INITIALIZER, useFactory: setAppConfig, deps: [AppConfigService], multi: true }
    ]
})
export class AppModule {
}

export function setAppConfig(appConfigService: AppConfigService) {
    const APP_CONFIG: AppConfig = {
        LanguageID: 1,
        CountryID: 1
    };

    return () => appConfigService.setAppConfigIfNotExists(APP_CONFIG);
}


export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

