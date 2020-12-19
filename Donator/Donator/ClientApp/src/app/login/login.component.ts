import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { BehaviorSubject } from "rxjs";
import { mergeMap } from "rxjs/operators";
import { UserToLogin } from "../shared/models/User/UsertoLogin";
import { AuthService } from "../shared/services/auth.service";
import { OrgUserService } from "../shared/services/org-user.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errorMessage: string;
  loginModel: UserToLogin = new UserToLogin('', '');
  signedIn$: BehaviorSubject<boolean>

  isSignedIn: boolean;
  userHasNPOs: boolean;

  constructor(
    private auth: AuthService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private orgusers: OrgUserService) {
    this.signedIn$ = this.auth.signedIn$;
  }

  ngOnInit() {}

  getUsersNPOList(userId: string) {
    this.orgusers.getListOfNPOsForUser(userId).subscribe(
      (results: any) => {
        if (results.status.statusCode === 200 && results.organizations.length >= 1) {
          this.userHasNPOs = true;
        } else {
          this.userHasNPOs = false;
        }
      },
      (err: any) => {
        console.log(err)
      }
    )
  }

  login() {
    this.spinner.show();
    if (this.loginModel.email != '' && this.loginModel.password != '') {
      this.auth.login(this.loginModel).subscribe(
        (data: any) => {
          this.spinner.hide();
          this.loginModel.email = '';
          this.loginModel.password = '';
        },
        (err: any) => {
          this.spinner.hide();
          if (err.status === 404) {
            this.errorMessage = err.error;
            this.loginModel.email = '';
            this.loginModel.password = '';
          }
        }
      );
    }
  }
}
