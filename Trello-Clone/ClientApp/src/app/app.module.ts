import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { SignupComponent } from './signup/signup.component';
import { SigninComponent } from './signin/signin.component';
import { ApplicationUserService } from './services/application.user.service';
import { UserBoardsComponent } from './userboards/userboards.component';
import { NavigationPanelComponent } from './navigationpanel/navigationpanel.component';
import { TeamService } from './services/team.service';

const appRoutes: Routes = [
  { path: 'signup', component: SignupComponent },
  { path: 'signin', component: SigninComponent },
  {
    path: '**', children: [
      {
        path: 'boards',
        component: UserBoardsComponent
      }
    ], component: UserBoardsComponent //TODO - here must be user account
  }
];

@NgModule({
  declarations: [
    AppComponent,
    SigninComponent,
    SignupComponent,
    UserBoardsComponent,
    NavigationPanelComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule
  ],
  providers: [
    ApplicationUserService,
    TeamService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
