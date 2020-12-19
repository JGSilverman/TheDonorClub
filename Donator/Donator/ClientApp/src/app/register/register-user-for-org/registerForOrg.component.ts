import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { NgxSpinnerService } from "ngx-spinner";
import { UserToJoinAsOrg } from "../../shared/models/User/UserToJoinasOrg";
import { AuthService } from "../../shared/services/auth.service";
const jwtHelper = new JwtHelperService();

@Component({
  selector: 'app-register-for-org',
  templateUrl: './register-for-org.component.html',
  styleUrls: ['./register-for-org.component.css']
})
export class RegisterForOrgComponent implements OnInit {
  @Output() moveToNextTab: EventEmitter<boolean> = new EventEmitter<boolean>();
  errMessage: string;
  decodedToken: any;
  userToJoinForOrg = new UserToJoinAsOrg('', '', '', '', '');
  joinAsOrgForm: FormGroup;
  userLoggedIn: string;

  constructor(private auth: AuthService,
    private fb: FormBuilder,
    public router: Router,
    private spinner: NgxSpinnerService) { }

  public ngOnInit() {
    this.userLoggedIn = this.auth.getUserId();
    this.createJoinAsOrgForm();
    if (this.userLoggedIn)
      this.moveToNextTab.emit(true);
  }

  public createJoinAsOrgForm() {
    this.joinAsOrgForm = this.fb.group({
      firstName: ["", [Validators.required]],
      lastName: ["", [Validators.required]],
      jobRole: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [
        Validators.required,
        Validators.minLength(6),
        this.patternValidator(/\d/, { hasNumber: true }),
        this.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        this.patternValidator(/[\[$@#!%*?&\]]/, { hasSpecialCharacters: true })
      ]]
    });
  }

  public joinAsOrg() {
    if (this.joinAsOrgForm.valid) {
      this.spinner.show();
      this.userToJoinForOrg.firstName = this.joinAsOrgForm.value.firstName.trim();
      this.userToJoinForOrg.lastName = this.joinAsOrgForm.value.lastName.trim();
      this.userToJoinForOrg.jobRole = this.joinAsOrgForm.value.jobRole.trim();
      this.userToJoinForOrg.email = this.joinAsOrgForm.value.email.trim();
      this.userToJoinForOrg.password = this.joinAsOrgForm.value.password.trim();
      this.auth.joinForOrg(this.userToJoinForOrg).subscribe(
        (data: any) => {
          this.spinner.hide();
          if (typeof (data.token) !== typeof (undefined) && data.token !== null && data.token !== "") {
            this.decodedToken = jwtHelper.decodeToken(data.token);
            localStorage.removeItem('_token');
            localStorage.removeItem('_uid');
            localStorage.removeItem('_u');
            localStorage.setItem('_token', data.token);
            localStorage.setItem('_uid', this.decodedToken.sub);
            localStorage.setItem('_u', data.user.userName);
            this.userLoggedIn = this.auth.getUserId();
          }
          if (this.userLoggedIn)
            this.moveToNextTab.emit(true);
        },
        (error: any) => {
          this.spinner.hide();
          console.log(error);
        });
    }
  }

  private patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.value) {
        // if control is empty return no error
        return null;
      }

      // test the value of the control against the regexp supplied
      const valid = regex.test(control.value);

      // if true, return no error (no error), else return error passed in the second parameter
      return valid ? null : error;
    };
  }
}
