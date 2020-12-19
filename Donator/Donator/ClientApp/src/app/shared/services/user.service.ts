import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json',
    'Authorization' : `Bearer ${localStorage.getItem('_token')}`
  })
}


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUri = environment.baseURI + '/users/';

  constructor(
    private http: HttpClient
  ) {}


  getUser(userId: string) {
    return this.http.get(this.baseUri + userId, httpOptions)
  }

}
