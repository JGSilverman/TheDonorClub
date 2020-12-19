import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { NgxSpinnerService } from 'ngx-spinner';
import { NPOToCreate } from 'src/app/shared/models/NPO/NPOtoCreate';
import { NPOType } from 'src/app/shared/models/NPOTypes/NPOType';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NPOTypesService } from 'src/app/shared/services/npo-types.service';
import { NPOService } from 'src/app/shared/services/npo.service';

@Component({
  selector: 'app-npo-new',
  templateUrl: './npo-new.component.html',
  styleUrls: ['./npo-new.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NpoNewComponent implements OnInit {
  @ViewChild('createNPOTabs', { static: false }) npoTabs: TabsetComponent;
  orgToBeCreated = new NPOToCreate('', 0, '', '', '', '');
  newOrgForm: FormGroup;
  userLoggedIn: string;
  npoTypes: NPOType[] = [];
  submitErrorMessage: string;

  constructor(
    public auth: AuthService,
    private fb: FormBuilder,
    public router: Router,
    private npoService: NPOService,
    private spinner: NgxSpinnerService,
    private npoTypesService: NPOTypesService) {
    this.userLoggedIn = this.auth.getUserId();
  }

  public selectTab(tabId: number) {
    this.npoTabs.tabs[tabId].active = true;
  }

  public ngOnInit() {
    this.createNewOrgForm();
    this.getNPOTypes();
  }

  public moveToNextTab(event) {
    if (event === true) {
      if (typeof (this.npoTabs) !== typeof (undefined) && this.npoTabs !== null) {
        this.npoTabs.tabs[0].disabled = true;
        this.npoTabs.tabs[1].disabled = false;
        this.npoTabs.tabs[0].active = false;
      }
      if (typeof (this.npoTabs) !== typeof (undefined) && this.npoTabs !== null) {
        setTimeout(() => {
          this.npoTabs.tabs[1].active = true;
        },
          1000);
      }
    }
  }

  public createNewOrgForm() {
    this.newOrgForm = this.fb.group({
      name: ["", Validators.required],
      typeId: ["", Validators.required],
      description: ['', Validators.required],
      websiteUrl: ['', Validators.required],
      taxId: ["", [Validators.required, Validators.minLength(9)]]
    });
  }

  public getNPOTypes() {
    this.npoTypesService.getNPOTypesList().subscribe(
      (data: any) => {
        if (data.status.statusCode === 200) {
          this.npoTypes = data.npoTypes;
        }
      });
  }

  public createOrg() {
    if (this.newOrgForm.valid) {
      this.orgToBeCreated.name = this.newOrgForm.value.name.trim();
      this.orgToBeCreated.description = this.newOrgForm.value.description.trim();
      this.orgToBeCreated.websiteUrl = this.newOrgForm.value.websiteUrl.trim();
      this.orgToBeCreated.typeId = parseInt(this.newOrgForm.value.typeId);
      this.orgToBeCreated.createdByUserId = this.auth.getUserId();
      this.orgToBeCreated.taxId = this.newOrgForm.value.taxId;
      this.npoService.createNPO(this.orgToBeCreated).subscribe(
        (result: any) => {
          if (result && result.success) {
            this.submitErrorMessage = null;
            this.router.navigateByUrl('/');
          } else {
            if (result.status.statusCode === 404)
              this.submitErrorMessage = "Tax Id provided is invalid!";
            else
              this.submitErrorMessage = result.message;
          }
          var btn = <HTMLButtonElement>document.getElementById("submit-button");
          btn.disabled = false;
          this.spinner.hide();
        },
        (error: any) => {
          this.spinner.hide();
          console.log(error);
        });
    }
  }

  public onSubmission(event: MouseEvent) {
    (event.target as HTMLButtonElement).disabled = true;
    this.spinner.show();
    this.createOrg();
  }

  public onTaxIdChanged(event: any) { }
}
