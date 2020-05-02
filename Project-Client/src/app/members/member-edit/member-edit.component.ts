import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
    selector: 'app-member-edit',
    templateUrl: './member-edit.component.html',
    styleUrls: ['./member-edit.component.css']
})
/** member-edit component*/
export class MemberEditComponent implements OnInit {

  user: User;
  @ViewChild('editForm') editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  /** member-edit ctor */
  constructor(private route: ActivatedRoute, private userService: UserService,
    private authService: AuthService, private alertService: AlertifyService) {

  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data["userData"];
    });

    if (this.user) {
      if (this.user.photourl === null) {
        this.user.photourl = "/assets/dummyImage.png";
      }
    }

    
  }

  updateUser() {

    this.userService.updateUser(this.authService.decodeToken.nameid, this.user).subscribe(next => {
      this.alertService.success("Profile update successfully!");
      this.editForm.reset(this.user);
    });

    
  }

  updateMemberPhoto(url) {

    this.user.photourl = url;

  }
}
