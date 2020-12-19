import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { INPOForList } from "../models/NPO/NPOForList";
import { AuthService } from "./auth.service";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json',
    'Authorization' : `Bearer ${localStorage.getItem('_token')}`
  })
}


@Injectable({
  providedIn: 'root'
})

export class OrgUserService {
  baseUri = environment.baseURI + '/api/orgusers/';

  constructor(private http: HttpClient, private auth: AuthService) {}


  getListOfNPOsForUser(userId: string): Observable<INPOForList[]> {
    return this.http.get<INPOForList[]>(this.baseUri + userId + '/npo-list', httpOptions).pipe(catchError(err => of(null)));
  }
}
