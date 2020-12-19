import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { PaginatedResult } from "../models/IPagination";
import { NPOToCreate } from "../models/NPO/NPOtoCreate";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json',
    'Authorization' : `Bearer ${localStorage.getItem('_token')}`
  })
}

@Injectable({
  providedIn: 'root'
})

export class NPOService {
  baseUri = environment.baseURI + '/api/npo/';

  constructor(
    private http: HttpClient
  ) {}


  createNPO(npo: NPOToCreate) {
    return this.http.post(this.baseUri + 'CreateNPO',  npo, httpOptions);
  }

  getNPOById(id: number) {
    return this.http.get(this.baseUri + id, httpOptions);
  }

  getNPOs(page?, itemsPerPage?) {
    const paginatedResult: PaginatedResult<any> = new PaginatedResult<any>();
    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get(this.baseUri + 'list', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  searchTaxIdGlobally(taxId: string): Observable<any> {
    return this.http.get<string>(this.baseUri + 'search/' + taxId, httpOptions);
  }
}
