import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBoardsComponent } from './userboards.component';

describe('UserboardsComponent', () => {
  let component: UserBoardsComponent;
  let fixture: ComponentFixture<UserBoardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UserBoardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBoardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
