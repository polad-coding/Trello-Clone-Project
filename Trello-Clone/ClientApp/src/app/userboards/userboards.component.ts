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
    this.applicationUserService.UserIsAuthenticated().subscribe((responce) => {
      this.applicationUserService.GetCurrentUser().subscribe((res) => {
        this.currentUser = <ApplicationUser>res.body;
      },
        (error) => { this.router.navigate(['/signup']) }
      ); 
    },
      (error) => { this.router.navigate(['/signup']) });
  }

}
