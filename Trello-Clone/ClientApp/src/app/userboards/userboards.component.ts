import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationUser } from '../models/application.user';
import { NavigationPanelComponent } from '../navigationpanel/navigationpanel.component';
import { ApplicationUserService } from '../services/application.user.service';

@Component({
  selector: 'app-userboards',
  templateUrl: './userboards.component.html',
  styleUrls: ['./userboards.component.css'],
  providers: [NavigationPanelComponent]
})
export class UserBoardsComponent implements OnInit {

  public currentUser: ApplicationUser;

  constructor(private applicationUserService: ApplicationUserService, private router: Router) { }

  ngOnInit() {
    this.applicationUserService.GetCurrentUser(localStorage.getItem('auth_token')).subscribe((response: any) => {
      if (response.body != null) {
        this.currentUser = <ApplicationUser>response.body;
        this.router.navigate([`/${response.body.username}/boards`]);
      }
      else {
        this.router.navigate(['/signup']);
      }
    });
  }
}
