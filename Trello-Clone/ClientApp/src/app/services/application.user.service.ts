import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationUser } from '../models/application.user';

@Injectable()
export class ApplicationUserService {

  //httpOptions: any = {
  //  headers: new HttpHeaders({
  //    'Access-Control-Allow-Origin': '*'
  //  }),
  //  observe: 'response' 
  //};

  constructor(private http: HttpClient) {}


  public ExistsByEmail(mail: string) {
    return this.http.get(`https://localhost:5001/api/authentication/ExistsByEmail/${mail}`, { observe: 'response' });

  }

  public ExistsByUserName(username: string) {
    return this.http.get(`https://localhost:5001/api/authentication/ExistsByUserName/${username}`, { observe: 'response' });

  }

  public CreateNewUser(newUser: ApplicationUser) {
    return this.http.post('https://localhost:5001/api/authentication/CreateNewUser', newUser, { observe: 'response' });
  }

  public LogInUser(user: ApplicationUser): Observable<HttpResponse<any>> {
    return this.http.post('https://localhost:5001/api/authentication/LogInUser', user, { observe: 'response' })
  }

  public UserIsAuthenticated() {
    return this.http.get('https://localhost:5001/api/authentication/UserIsAuthenticated', { observe: 'response' })
  }

  public GetCurrentUser() {
    return this.http.get('https://localhost:5001/api/authentication/GetCurrentUser', { observe: 'response' });
  }
}
