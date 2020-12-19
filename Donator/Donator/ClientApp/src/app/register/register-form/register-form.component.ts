import { AfterViewInit, Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { NavigationEnd, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { UserToSignUp } from "src/app/shared/models/User/UsertoSignUp";
import { AuthService } from "src/app/shared/services/auth.service";
import { UserToJoinAsOrg } from "../../shared/models/User/UserToJoinasOrg";

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent implements OnInit {

  newUserToRegister = new UserToSignUp('', '');

  registerUserForm: FormGroup;

  constructor(
    private auth: AuthService,
    private fb: FormBuilder,
    public router: Router,
    private spinner: NgxSpinnerService) {}

  ngOnInit() {this.createGeneralRegisterForm();}


  createGeneralRegisterForm() {
     this.registerUserForm = this.fb.group({
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

  register() {
    if (this.registerUserForm.valid) {
      this.spinner.show();
      this.newUserToRegister.email = this.registerUserForm.value.email.trim();
      this.newUserToRegister.password = this.registerUserForm.value.password.trim();

      this.auth.signUp(this.newUserToRegister).subscribe(
        (data: any) => {
          this.spinner.hide();
          this.router.navigate(['/login']);
        },
        (err: any) => {
          this.spinner.hide();
          console.log(err);
        }
      );
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
