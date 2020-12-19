import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NPOType } from '../shared/models/NPOTypes/NPOType';
import { AuthService } from '../shared/services/auth.service';
import { NPOTypesService } from '../shared/services/npo-types.service';
import { NPOService } from '../shared/services/npo.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userId$: BehaviorSubject<string>
  userName$: BehaviorSubject<string>
  signedIn$: BehaviorSubject<boolean>
  npoTypes: NPOType[] = [];

  selectedNPOType: NPOType;

  constructor(private auth: AuthService, private npoService: NPOService, private npoTypesService: NPOTypesService) {
    this.signedIn$ = this.auth.signedIn$;
    this.userName$ = this.auth.userName$;
    this.userId$ = this.auth.userId$;
  }

  ngOnInit() {
    this.getNPOTypes();
  }

  getNPOTypes() {
    this.npoTypesService.getNPOTypesList().subscribe(
      (data: any) => {
        if (data.status.statusCode === 200) {
          this.npoTypes = data.npoTypes;
        }
      });
  }

}
