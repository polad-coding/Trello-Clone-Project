import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationUser } from '../models/application.user';
import { map } from 'rxjs/operators';

@Injectable()
export class ApplicationUserService {

  constructor(private http: HttpClient) { }


  public ExistsByEmail(mail: string) {
    return this.http.get(`https://localhost:5001/api/authentication/ExistsByEmail/${mail}`, { observe: 'response' });

  }

  public ExistsByUserName(username: string) {
    return this.http.get(`https://localhost:5001/api/authentication/ExistsByUserName/${username}`, { observe: 'response' });

  }

  public CreateNewUser(newUser: ApplicationUser) {
    return this.http.post('https://localhost:5001/api/authentication/CreateNewUser', newUser, { observe: 'response' });
  }

  public LogInUser(user: ApplicationUser) {
    return this.http.post('https://localhost:5001/api/authentication/LogInUser', user, { observe: 'response' });
  }

  public GetCurrentUser(token: string) {
    return this.http.get('https://localhost:5001/api/authentication/GetCurrentUser', { headers: { 'Authorization': `Bearer ${token}` }, observe: 'response'  });
  }

  public GetJwtToken(user: ApplicationUser) {
    return this.http.post(`https://localhost:5001/api/authentication/GetJwtToken`, user, { observe: 'response' });
  }

  public LogOutUser(token: string) {
    return this.http.post('https://localhost:5001/api/authentication/LogOutUser', null, { headers: { 'Authorization': `Bearer ${token}` }, observe: 'response' });

  }
}
