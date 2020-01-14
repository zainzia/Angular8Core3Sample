import { Injectable } from '@angular/core';

import {AppConfig} from './../interfaces/Home/app-config.module'

@Injectable()
export class AppConfigService {


    getAppConfig(): AppConfig {
        return JSON.parse(localStorage.getItem('APP_CONFIG'));
    }


    setAppConfig(appConfig: AppConfig) {
        localStorage.setItem('APP_CONFIG', JSON.stringify(appConfig));
    }


    setAppConfigIfNotExists(appConfig: AppConfig) {
        if (!this.getAppConfig()) {
            this.setAppConfig(appConfig);
        }
    }
}
