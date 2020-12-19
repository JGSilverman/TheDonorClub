import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { INPOForList } from 'src/app/shared/models/NPO/NPOForList';
import { AuthService } from 'src/app/shared/services/auth.service';
import { OrgUserService } from 'src/app/shared/services/org-user.service';

@Component({
  selector: 'app-npo-signin',
  templateUrl: './npo-signin.component.html',
  styleUrls: ['./npo-signin.component.css']
})
export class NPOSignInComponent implements OnInit {
  npoList: INPOForList[] = [];

  constructor(
    private orgusers: OrgUserService,
    private router: Router,
    private authentication: AuthService) { }

  ngOnInit() {
    this.getUsersNPOList(this.authentication.getUserId())
  }

  getUsersNPOList(userId: string) {
    this.orgusers.getListOfNPOsForUser(userId).subscribe(
      (results: any) => {
        if (results.status.statusCode === 200 && results.organizations.length >= 1) {
          this.npoList = results.organizations;
        }
      },
      (err: any) => {
        console.log(err)
      }
    )
  }

  routeToNPODashboard(id: number) {
    this.router.navigateByUrl('/npo/' + id + '/dashboard');
  }

}
