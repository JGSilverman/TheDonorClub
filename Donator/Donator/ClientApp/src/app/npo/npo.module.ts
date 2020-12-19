import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NpoComponent } from './npo/npo.component';
import { NpoNewComponent } from './npo-new/npo-new.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { NpoDashboardComponent } from './dashboard/npo-dashboard.component';
import { NPOSignInComponent } from './npo-sign-in/npo-signin.component';
import { NPOSettingsComponent } from './settings/npo-settings.component';
import { RegisterForOrgComponent } from "../register/register-user-for-org/registerForOrg.component";
import { NPORoutingModule } from './npo-routing.module';


@NgModule({
  declarations: [
    NpoComponent,
    NpoNewComponent,
    NpoDashboardComponent,
    NPOSignInComponent,
    NPOSettingsComponent,
    RegisterForOrgComponent
  ],
  imports: [
    NPORoutingModule,
    SharedModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    TabsModule.forRoot()
  ],
  providers: [],
  schemas: []
})
export class NPOModule { }
