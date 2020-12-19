import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { INPO } from 'src/app/shared/models/NPO/INPO';

@Component({
  selector: 'app-npo-dashboard',
  templateUrl: './npo-dashboard.component.html',
  styleUrls: ['./npo-dashboard.component.css']
})
export class NpoDashboardComponent implements OnInit {
  npo: INPO;
  constructor(private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activeRoute.data.subscribe(data => {
      this.npo = data["npo"];
    });
    console.log(this.npo)
  }

}
