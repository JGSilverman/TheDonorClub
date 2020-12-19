export class UserAuth {
  token: string;
  user: {
    id: string;
    userName: string;
  }

  constructor(token: string, id: string, username: string) {
    this.token = token;
    this.user.id = id;
    this.user.userName = username;
  }
}
