import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { INPO } from "../models/NPO/INPO";
import { NPOService } from "../services/npo.service";


@Injectable()
export class NPOResolver implements Resolve<INPO> {
  constructor(
    private service: NPOService,
    private router: Router,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<INPO> {
    return this.service
      .getNPOById(route.params["id"])
      .pipe(
        map((npo: any) => {
          if (npo.status.statusCode === 200) {
            return npo;
          }
          else {
            this.router.navigate(["/"]);
          }
        }),
        catchError(error => {
          this.router.navigate(["/home"]);
          return of(null);
        })
      );
  }
}
