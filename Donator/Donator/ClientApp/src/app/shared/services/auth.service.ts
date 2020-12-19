import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { environment } from "src/environments/environment";
import { UserToLogin } from "../models/User/UsertoLogin";
import { catchError, flatMap, map, mergeMap, shareReplay, switchMap } from 'rxjs/operators';
import { BehaviorSubject, from, Observable, of } from "rxjs";
import { UserAuth } from "../models/User/UserUath";
import { Router } from "@angular/router";
import { UserToSignUp } from "../models/User/UsertoSignUp";
import { UserToJoinAsOrg } from "../models/User/UserToJoinasOrg";

const jwtHelper = new JwtHelperService();
// const expirationDate = jwtHelper.getTokenExpirationDate(myRawToken);
// const isExpired = jwtHelper.isTokenExpired(myRawToken);

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userId$ = new BehaviorSubject<string>('');
  userName$ = new BehaviorSubject<string>('');
  signedIn$ = new BehaviorSubject<boolean>(false);
  decodedToken: any;
  baseUri = environment.baseURI + '/api/auth/';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.userId$ = new BehaviorSubject<string>(localStorage.getItem('_uid'));
  }

  public signUp(user: UserToSignUp) {
    return this.http.post(this.baseUri + 'SignUp', user)
                    .pipe(
                      map((resp: any) => {
                        if (resp.status.statusCode === 201) {
                        } else {
                          console.log('something happened');
                        }
                      })
                    );
  }

  public joinForOrg(user: UserToJoinAsOrg) {
    return this.http.post(this.baseUri + 'join', user);
  }

  public login(user: UserToLogin) {
    return this.http.post(this.baseUri + 'SignIn', user)
          .pipe(
            map((resp: UserAuth) => {
              if (resp) {
                this.decodedToken = jwtHelper.decodeToken(resp.token);
                localStorage.setItem('_token', resp.token);
                localStorage.setItem('_uid', this.decodedToken.sub);
                localStorage.setItem('_u', resp.user.userName);
                this.userId$.next(resp.user.id);
                this.userName$.next(resp.user.userName);
                this.signedIn$.next(true);
                this.router.navigate(['/']);
              } else {
                console.log('something happened');
              }
            }),
            catchError(err => of(null))
          );
  }

  public logout() {
    localStorage.removeItem('_token');
    localStorage.removeItem('_uid');
    localStorage.removeItem('_u');
    this.userId$.next(null);
    this.userName$.next(null);
    this.signedIn$.next(false);
  }

  public getUserId() {
    return localStorage.getItem("_uid");
  }

  public isLoggedIn() {
    const token = localStorage.getItem('_token');
    if (token) {
      this.signedIn$.next(true);
    } else {
      this.signedIn$.next(false);
    }
  }
}
