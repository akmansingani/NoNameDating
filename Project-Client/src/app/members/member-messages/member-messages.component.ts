import { Component, OnInit, Input } from '@angular/core';
import { Message } from '../../_models/message';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-member-messages',
    templateUrl: './member-messages.component.html',
    styleUrls: ['./member-messages.component.css']
})
/** member-messages component*/
export class MemberMessagesComponent implements OnInit {

  @Input() receiverID: number;
  messages: Message[];
  newMessage: any = {};

  /** member-messages ctor */
  constructor(private userService: UserService, private authService: AuthService, private alertService: AlertifyService) {

  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {

    const currentUser = +this.authService.decodeToken.nameid;

    this.userService.getMessageThread(this.authService.decodeToken.nameid, this.receiverID)
      .pipe(
        tap(messages => {
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].receiverID === currentUser) {
              this.userService.readMessage(currentUser, messages[i].messageID);
            }
          }
        })
      )
      .subscribe(resp => {
      this.messages = resp;
    }, error => {
        this.alertService.error("Request Failed");
    })
  }

  sendMessage() {
    this.newMessage.receiverID = this.receiverID;
    this.userService.sendMessage(this.authService.decodeToken.nameid, this.newMessage).subscribe((response: Message) => {

      this.messages.unshift(response);
      this.newMessage = {};

    }, error => {
        this.alertService.error("Send Message Failed");
    });
  }




}
