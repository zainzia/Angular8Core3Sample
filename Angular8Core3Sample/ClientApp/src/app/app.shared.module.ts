import { NgModule } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { PlatformModule } from '@angular/cdk/platform';


import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { MasonryLayoutComponent } from './components/masonry-layout/masonry-layout.component';
import { ImageSlideShowComponent } from './components/image-slide-show/image-slide-show.component';
import { LoaderComponent } from './components/loader/loader.component';
import { RegisterComponent } from './components/register/register.component'
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { ListsComponent } from './components/lists/lists.component';


import { AuthInterceptor } from './services/auth.interceptor';
import { AuthResponseInterceptor } from './services/auth.response.interceptor';
import { CountryService } from './services/country.service';
import { LanguageService } from './services/language.service';
import { MyListsService } from './services/myLists.service';
import { LoaderService } from './services/loader.service';
import { AppConfigService } from './services/appConfig.service';
import { BreadcrumbService } from './services/breadcrumb.service';

import { FilterProductDescriptionsByLanguagePipe } from './pipes/filter-descriptions.module';


import { ButtonModule } from 'primeng/components/button/button';
import { InputTextModule } from 'primeng/inputtext';
import { SharedModule, ConfirmDialogModule, MessageService } from 'primeng/primeng';
import { DialogModule } from 'primeng/primeng';
import { DropdownModule } from 'primeng/dropdown';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { SliderModule } from 'primeng/slider';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService } from 'primeng/api';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { BreadcrumbModule } from 'primeng/breadcrumb';




@NgModule({
    declarations: [
      AppComponent,
      HomeComponent,
      LoaderComponent,
      FilterProductDescriptionsByLanguagePipe,
      MasonryLayoutComponent,
      ImageSlideShowComponent,
      ListsComponent,
      ConfirmDialogComponent
    ],
    imports: [
        CommonModule,
        ScrollingModule,
        PlatformModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        ConfirmDialogModule,
        InputTextModule,
        BrowserAnimationsModule,
        FormsModule,
        DynamicDialogModule,
        MatProgressBarModule,
        MatBadgeModule,
        CardModule,
        ToastModule,
        SharedModule,
        ButtonModule,
        SliderModule,
        CheckboxModule,
        ReactiveFormsModule,
        BreadcrumbModule,
        DropdownModule,
        DialogModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'Home', pathMatch: 'full' },
            { path: 'Home', component: HomeComponent },
            { path: 'Index', redirectTo: 'Home', pathMatch: 'full' },
            { path: 'Register', component: RegisterComponent },
            {
                path: 'MyAccount/:id',
                children: [
                  { path: 'MyLists', component: ListsComponent }
                ]
            },
            { path: '**', redirectTo: 'Home' }
        ])
    ],
    providers: [
      CountryService,
      LoaderService,
      LanguageService,
      MyListsService,
      AppConfigService,
      BreadcrumbService,
      ConfirmationService,
      MessageService,
      {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true
      },
      {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthResponseInterceptor,
        multi: true
      },
      {
          provide: Window,
          useValue: window
      }
    ],
    entryComponents: [
        ConfirmDialogComponent
    ]
})


export class AppModuleShared {
}
