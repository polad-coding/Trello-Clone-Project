import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TeamModel } from '../models/team';

@Injectable()
export class BoardService {

  authToken: string = localStorage.getItem('auth_token');

  constructor(private http: HttpClient) {

  }

  public GetBackgroudColorSchemes() {
    return this.http.get('https://localhost:5001/api/board/GetBackgroundColorSchemes', { headers: { 'Authentication': `Bearer ${this.authToken}` }, observe: 'response' });
  }

}
