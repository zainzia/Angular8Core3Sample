import { enableProdMode, ViewEncapsulation } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.browser.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

// Enables Hot Module Replacement.
declare var module: any;

if (module.hot) {
  module.hot.accept();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule).catch(err => console.log(err));
