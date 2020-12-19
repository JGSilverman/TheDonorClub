import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { NPOToCreate } from "../models/NPO/NPOtoCreate";
import { NPOType } from "../models/NPO/NPOType";
import { NPOTypeToCreate } from "../models/NPOTypes/NPOTypeToCreate";
import { NPOTypeToUpdate } from "../models/NPOTypes/NPOTypeToUpdate";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json',
    'Authorization' : `Bearer ${localStorage.getItem('_token')}`
  })
}

@Injectable({
  providedIn: 'root'
})

export class NPOTypesService {
  baseUri = environment.baseURI + '/api/npotypes/';

  constructor(
    private http: HttpClient
  ) {}

  getNPOTypeById(typeId: number): Observable<NPOType> {
    return this.http.get<NPOType>(this.baseUri + typeId, httpOptions);
  }

  getNPOTypesList(): Observable<NPOType[]> {
    return this.http.get<NPOType[]>(this.baseUri, httpOptions);
  }

  createNPOType(type: NPOTypeToCreate) {
    return this.http.post(this.baseUri + 'CreateNPOType',  type, httpOptions);
  }

  updateNPOType(type: NPOTypeToUpdate) {
    return this.http.post(this.baseUri + type.id,  type, httpOptions);
  }

  deleteNPOType(typeId: number) {
    return this.http.delete(this.baseUri + typeId, httpOptions);
  }
}
