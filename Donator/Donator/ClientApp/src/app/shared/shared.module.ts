import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NPOService } from './services/npo.service';
import { NPOTypesService } from './services/npo-types.service';
import { OrgUserService } from './services/org-user.service';
import { NPOResolver } from './resolvers/npo.resolver';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthService,
    UserService,
    NPOService,
    OrgUserService,
    NPOTypesService,
    NPOResolver
  ],
  exports: []
})
export class SharedModule { }
