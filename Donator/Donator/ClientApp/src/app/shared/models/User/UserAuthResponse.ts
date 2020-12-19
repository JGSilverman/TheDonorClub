export interface IUserAuthResponse {
  status: {
    statusCode: number
  },
  user: {
    id: string;
    userName: string;
  }
}
