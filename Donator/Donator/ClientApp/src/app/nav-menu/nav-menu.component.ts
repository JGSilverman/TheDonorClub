import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  userId$: BehaviorSubject<string>
  userName$: BehaviorSubject<string>
  signedIn$: BehaviorSubject<boolean>

  constructor(private auth: AuthService, private router: Router) {
    this.signedIn$ = this.auth.signedIn$;
    this.userName$ = this.auth.userName$;
    this.userId$ = this.auth.userId$;
  }

  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/');
  }

  goToHome() {
    this.router.navigateByUrl('/');
  }

  goToNPOJoin() {
    this.router.navigateByUrl('/npo/join');
  }
}
