import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './shared/services/auth.service';
import { OrgUserService } from './shared/services/org-user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  userId$: BehaviorSubject<string>
  userName$: BehaviorSubject<string>
  signedIn$: BehaviorSubject<boolean>
  onNPOJoinPage: boolean = false;

  constructor(
    private auth: AuthService,
    private router: Router,
    private orgusers: OrgUserService) {
    this.signedIn$ = this.auth.signedIn$;
    this.userName$ = this.auth.userName$;
    this.userId$ = this.auth.userId$;
  }

  ngOnInit() {
    this.auth.isLoggedIn();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (event.url.includes('npo/join')) {
          this.onNPOJoinPage = true;
        } else {
          this.onNPOJoinPage = false;
        }
      }
    });

    this.auth.signedIn$.subscribe(data => {
      if (data) {
        this.getUsersNPOList(this.auth.getUserId());
      }
    })
  }


  getUsersNPOList(userId: string) {
    this.orgusers.getListOfNPOsForUser(userId).subscribe(
      (results: any) => {
        if (results.status.statusCode === 200 && results.organizations.length >= 1) {
          // redirect to npo list
          this.router.navigate(['npo/signin']);
        }
      },
      (err: any) => {
        console.log(err)
      }
    )
  }
}
