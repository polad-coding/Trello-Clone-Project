import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TeamModel } from '../models/team';

@Injectable()
export class TeamService {

  authToken: string = localStorage.getItem('auth_token');

  constructor(private http: HttpClient) {

  }

  public CreateTeam(teamModel: TeamModel) {
    return this.http.post('https://localhost:5001/api/team/CreateTeamAsync', teamModel, { observe: 'response', headers: {'Authorization' : `Bearer ${this.authToken}`} });
  }

  public AssociateUserWithTeam(teamId: number) {
    return this.http.post('https://localhost:5001/api/team/AssociateUserWithTeamAsync', teamId, { observe: 'response', headers: { 'Authorization': `Bearer ${this.authToken}` } });
  }

}
