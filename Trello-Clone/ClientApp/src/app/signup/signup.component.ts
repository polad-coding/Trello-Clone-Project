import { Component, ElementRef, OnInit, Renderer2, ViewChild, ViewChildren, QueryList, HostBinding } from '@angular/core';
import { FormControl, FormGroup, NgForm, NgModel, Validators } from '@angular/forms';
import { ApplicationUser } from '../models/application.user';
import { ApplicationUserService } from '../services/application.user.service';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';
import { error } from 'protractor';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  providers: [ApplicationUserService],
  host: {
    '(document:click)': 'DocumentClicked()',
  },
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
  ]
})
export class SignupComponent {

  public continueButtonIsActive = false;
  public passwordIsVisible = false;
  public errorMessageIsVisisble = false;
  public firstStagePassed = false;
  public submitted = false;
  public passwordComplexity = -1;
  public passwordComplexityIndicator: string = "";
  public newUser: ApplicationUser = new ApplicationUser();

  @ViewChildren('passwordCompexityContainer', { read: ElementRef })
  public passwordComplexityContainer: QueryList<ElementRef>;
  @ViewChildren('passwordField', { read: ElementRef })
  public passwordFields: QueryList<ElementRef>;
  @ViewChildren('password', { read: ElementRef })
  public passwords: QueryList<ElementRef>;


  constructor(private applicationUserService: ApplicationUserService, private renderer: Renderer2, private router: Router) { }

  /**
   * Toggle password field visibility on eye icon pressed.
   * @param password
   */
  public ToggleFieldVisibility(password: NgModel) {
    this.passwordIsVisible = !this.passwordIsVisible;
    if (this.passwordIsVisible) {
      this.passwords.forEach((current) => {
        this.renderer.setAttribute(current.nativeElement, 'type', 'text');
      });
    }
    else {
      this.passwords.forEach((current) => {
        this.renderer.setAttribute(current.nativeElement, 'type', 'password');
      });
    }
  }

  /**
   * If document is clicked remove error messages by setting the 'submitted' flag to false.
   * */
  public DocumentClicked() {
    if (this.passwordFields) {
      this.passwordFields.forEach((current) => {
        this.renderer.setStyle(current.nativeElement, 'border-color', '#DFE1E6');
      });
    }
    this.submitted = false;
  }

  /**
   * Focus password element.
   * @param event
   * @param element
   */
  public FocusElement(event: MouseEvent, element: HTMLElement) {
    event.stopPropagation();
    this.submitted = false;
    element.style.borderColor = '#0077b6';
  }

  /**
   * Check if mail input is correct and respectively disable or enable 'continue' button.
   * @param event
   * @param form
   */
  public IsMailFieldInputCorrect(event: KeyboardEvent, form: NgForm) {
    if (event.keyCode !== 13) {
      this.errorMessageIsVisisble = false;
    }

    if (form.valid) {
      this.continueButtonIsActive = true;
    }
    else {
      this.continueButtonIsActive = false;
    }
  }

  /**
   * Check if mail is not registered already and proceed in this case to second registration stage.
   * @param form
   */
  public ProceedToSecondReistrationStage(form: NgForm) {
    this.applicationUserService.ExistsByEmail(form.value.email).subscribe((response) => {
      if (response.body == true) {
        this.errorMessageIsVisisble = true;
        this.continueButtonIsActive = false;
      }
      else {
        this.firstStagePassed = true;
      }
    });
  }

  /**
   * Create new user and proceed to user account component.
   * @param form
   * @param event
   */
  public CreateNewUser(form: NgForm, event: MouseEvent) {
    event.stopPropagation();
    this.submitted = true;

    if (form.valid) {
      this.applicationUserService.ExistsByUserName(form.value.username).subscribe((existsByUserNameResponse) => {
        if (existsByUserNameResponse.body == true) {
          form.controls['username'].setErrors({ 'alreadyExists': true })
        }
        else {
          this.applicationUserService.CreateNewUser(this.newUser).subscribe((createNewUserResponse: any) => {
            this.applicationUserService.LogInUser(createNewUserResponse.body).subscribe((logInResponse: any) => {
              this.applicationUserService.GetJwtToken(logInResponse.body).subscribe((getJwtResponse: any) => {
                localStorage.setItem('auth_token', getJwtResponse.body);
                this.router.navigate([`/${logInResponse.body.username}/boards`]);
              })
            });
          });
        }
      });
    }
  }

  /**
   * Go back to the first registration stage.
   * @param form
   */
  public BackToEmailSection(form: NgForm) {
    this.firstStagePassed = !this.firstStagePassed;
    this.continueButtonIsActive = false;
    form.reset()
  }

  public CheckPasswordComplexity(element: NgModel) {
    this.passwordComplexity = -1;
    this.passwordComplexityIndicator = " ";
    if (element.value.length >= 8) {
      this.passwordComplexity++;
      if (new RegExp('[A-Z]').test(element.value)) {
        this.passwordComplexity++;
      }
      if (new RegExp('[0-9]').test(element.value)) {
        this.passwordComplexity++;
      }
      if (element.value.length >= 12) {
        this.passwordComplexity++;

      }
      if (new RegExp('[^A-Za-z0-9]').test(element.value)) {
        this.passwordComplexity++;
      }
    }
    this.ChangePasswordComplexityIndicator();
  }

  public ChangePasswordComplexityIndicator() {
    this.passwordComplexityContainer.forEach((current) => {
      for (var i = 0; i < current.nativeElement.children.length; i++) {
        if (i <= this.passwordComplexity) {
          if (this.passwordComplexity == 0) {
            this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(191, 38, 0)");
            this.passwordComplexityIndicator = "Week";
          }
          else if (this.passwordComplexity == 1) {
            this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(255, 86, 48)");
            this.passwordComplexityIndicator = "Fair";
          }
          else if (this.passwordComplexity == 2) {
            this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(255, 171, 0)");
            this.passwordComplexityIndicator = "Good";

          }
          else if (this.passwordComplexity == 3) {
            this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(54, 179, 126)");
            this.passwordComplexityIndicator = "Strong";
          }
          else if (this.passwordComplexity == 4) {
            this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(0, 135, 90)");
            this.passwordComplexityIndicator = "Very strong";
          }
        }
        else {
          this.renderer.setStyle(current.nativeElement.children[i], "border-color", "rgb(223, 225, 230)");
        }
      }
    });

  }

}
