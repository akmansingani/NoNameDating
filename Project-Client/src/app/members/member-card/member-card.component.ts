import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../_models/user';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';

@Component({
    selector: 'app-member-card',
    templateUrl: './member-card.component.html',
    styleUrls: ['./member-card.component.css']
})
/** member-card component*/
export class MemberCardComponent implements OnInit {

  @Input() user: User;

  /** member-card ctor */
  constructor(private authService: AuthService, private alertService: AlertifyService,
    private userService: UserService) {
      
  }

  ngOnInit() {
   
  }

  likeUser(likedUser: number) {

    this.userService.likeUser(this.authService.decodeToken.nameid, likedUser).subscribe(data => {
      this.alertService.success("Like success!");
    }, error => {
      this.alertService.error(error);
    });
  }
}
