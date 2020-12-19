// CORE ANGULAR IMPORTS
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// FORMS IMPORTS
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// SHARED MODULE
import { SharedModule } from './shared/shared.module';

// SPINNER
import { NgxSpinnerModule } from "ngx-spinner";

// COMPONENTS
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RegisterComponent } from './register/register.component';
import { RegisterFormComponent } from './register/register-form/register-form.component';
import { LoginComponent } from './login/login.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    RegisterFormComponent,
    LoginComponent,
    NavMenuComponent
  ],
  imports: [
    SharedModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [RegisterComponent, RegisterFormComponent, LoginComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule { }
