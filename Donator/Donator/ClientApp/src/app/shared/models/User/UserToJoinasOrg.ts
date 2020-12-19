export class UserToJoinAsOrg {
  firstName: string;
  lastName: string;
  jobRole: string;
  email: string;
  password: string;

  constructor(
    firstname: string, lastname: string, role: string,
    email: string, password: string
  ) {
    this.firstName = firstname;
    this.lastName = lastname;
    this.jobRole = role;
    this.email = email;
    this.password = password;
  }
}
