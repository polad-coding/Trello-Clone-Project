import { animate, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { error } from 'protractor';
import { Observable } from 'rxjs';
import { ApplicationUser } from '../models/application.user';
import { ApplicationUserService } from '../services/application.user.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
  animations: [
    trigger('InsertRemoveTrigger', [
      transition(':enter', [
        style({ transform: 'translateX(200%)' }),
        animate('100ms', style({ transform: 'translateX(0%)' })),
      ]),
      transition(':leave', [
        animate('0ms', style({ opacity: 0 }))
      ])
    ])
  ],
  host: {
    '(document:click)': 'DocumentClicked()',
  },
  providers: [ApplicationUserService]
})
export class SigninComponent{

  public user: ApplicationUser = new ApplicationUser();
  public submitted = false;

  constructor(private applicationUserService: ApplicationUserService,private router: Router) { }

  /**
   * If document is clicked remove error messages by setting the 'submitted' flag to false.
   *  */
  public DocumentClicked() {
    this.submitted = false;
  }

  /**
   * Check if form is correct, if so sign in user; else display error messages.
   * @param form
   */
  public SubmitForm(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.applicationUserService.ExistsByEmail(form.value.email).subscribe((existsByEmailResponse) => {
        if (existsByEmailResponse.body == true) {
          this.applicationUserService.LogInUser(this.user).subscribe((logInResponse: any) => {
            this.applicationUserService.GetJwtToken(logInResponse.body).subscribe((getJwtResponse: any) => {
              localStorage.setItem('auth_token', getJwtResponse.body);
                this.router.navigate([`/${logInResponse.body.username}/boards`]);
            })
          },
            (error) => {
              form.controls['password'].setErrors({ 'incorrectPassword': true });
            });
        }
        else {
          form.controls['email'].setErrors({ 'doesntExists': true });
        }
      });
    }
  }

  /**
   * When anchor is clicked, redirect to signup page.
   * */
  public RedirectToSignUpPage() {
    this.router.navigate(['/signup']);
  }

}
