import { Component, OnInit, AfterViewInit, AfterContentInit, Input, OnChanges, Renderer2, ViewChild, ElementRef, ViewChildren, QueryList, } from '@angular/core';
import { ApplicationUser } from '../models/application.user';
import { TeamModel } from '../models/team';
import { NgForm, NgModel } from '@angular/forms';
import { TeamService } from '../services/team.service';
import { ApplicationUserService } from '../services/application.user.service';
import { Router } from '@angular/router';
import { BoardService } from '../services/board.service';

@Component({
  selector: 'app-navigationpanel',
  templateUrl: './navigationpanel.component.html',
  styleUrls: ['./navigationpanel.component.css'],
  host: {
    '(document:click)': 'DocumentClicked()',
  },
  providers: [TeamService]

})
export class NavigationPanelComponent implements OnInit, OnChanges {

  @Input() user: any;
  public userInitials: string;
  public createModalIsVisisble: boolean = false;
  public accountModalIsVisible: boolean = false;
  public teamModalIsVisible: boolean = false;
  public boardModalIsVisible: boolean = false;
  public newTeam: TeamModel = new TeamModel();
  public submitButtonIsActive: boolean = false;
  public boardsBackgroundColors: Array<string>;
  @ViewChild('boardCreationPart', { static: false })
  public boardCreationPartElement: ElementRef;

  constructor(private renderer: Renderer2,
    private teamService: TeamService,
    private applicationUserService: ApplicationUserService,
    private router: Router,
    private boardService: BoardService) { }

  ngOnInit() {
    this.boardService.GetBackgroudColorSchemes().subscribe((response: any) => {
      this.boardsBackgroundColors = response.body;

    });
  }

  public PreventEventBubblingOnTeamModal(event: MouseEvent) {
    event.stopPropagation();
  }

  public DocumentClicked() {
    this.createModalIsVisisble = false;
    this.accountModalIsVisible = false;
    this.teamModalIsVisible = false;
    this.boardModalIsVisible = false;
  }

  public CloseTeamModal() {
    this.teamModalIsVisible = false;
  }

  ngOnChanges() {
    this.renderer.setStyle(this.boardCreationPartElement.nativeElement, 'background-color', this.boardsBackgroundColors[0]);

    let initials = this.user.fullName.split(' ').map((str) => { return str.charAt(0); })
    this.userInitials = `${initials[0] != undefined ? initials[0] : ''}${initials[1] != undefined ? initials[1] : ''}`;
  }

  public ToggleCreateModalVisibility(event: MouseEvent) {
    event.stopPropagation();
    this.createModalIsVisisble = !this.createModalIsVisisble;
    this.accountModalIsVisible = false;
  }

  public ToggleAccountModalVisibility(event: MouseEvent) {
    event.stopPropagation();
    this.accountModalIsVisible = !this.accountModalIsVisible;
    this.createModalIsVisisble = false;
  }

  public DisplayCreateTeamModal(event: MouseEvent) {
    event.stopPropagation();
    this.createModalIsVisisble = false;
    //display create team modal
    this.teamModalIsVisible = true;
  }

  public DisplayCreateBoardModal(event: MouseEvent) {
    event.stopPropagation();
    this.createModalIsVisisble = false;
    this.boardModalIsVisible = true;

  }

  public CheckFieldLength(teamName: NgModel) {
    if (teamName.value.length > 0) {
      this.submitButtonIsActive = true;
    }
    else {
      this.submitButtonIsActive = false;
    }
  }

  public SubmitForm(teamCreationForm: NgForm) {
    this.teamService.CreateTeam(this.newTeam).subscribe((responce: any) => {
      this.teamService.AssociateUserWithTeam(responce.body.id).subscribe((res) => {
        teamCreationForm.reset();
        this.newTeam.id = responce.body.id;
        this.teamModalIsVisible = false;
      });
    });
  }

  public StopFurtherEventPropagation(event: MouseEvent) {
    event.stopPropagation();
  }

  public LogOutUser() {
    let token = localStorage.getItem('auth_token');
    this.applicationUserService.LogOutUser(token).subscribe((response) => {
      localStorage.removeItem('auth_token');
      this.router.navigate(['/signup']);
    });
  }
}
