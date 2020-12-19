import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NPOResolver } from '../shared/resolvers/npo.resolver';
import { NpoDashboardComponent } from './dashboard/npo-dashboard.component';
import { NpoNewComponent } from './npo-new/npo-new.component';
import { NPOSignInComponent } from './npo-sign-in/npo-signin.component';


const routes: Routes = [
    { path: 'join', component: NpoNewComponent },
    { path: 'signin', component: NPOSignInComponent },
    {
      path: ':id',
      children: [
        { path: 'dashboard', component: NpoDashboardComponent, resolve: { npo: NPOResolver }},
        { path: 'settings', component: NPOSignInComponent }
      ]
    }
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NPORoutingModule { }
