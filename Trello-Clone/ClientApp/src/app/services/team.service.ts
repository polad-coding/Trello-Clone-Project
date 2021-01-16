import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TeamModel } from '../models/team';

@Injectable()
export class TeamService {

  constructor(private http: HttpClient) {

  }

  public CreateTeam(teamModel: TeamModel) {
    return this.http.post('https://localhost:44373/api/team/CreateTeamAsync', teamModel, { observe: 'response' });
  }

  public AssociateUserWithTeam(teamId: number) {
    return this.http.post('https://localhost:44373/api/team/AssociateUserWithTeamAsync', teamId, { observe: 'response' });
  }

}
