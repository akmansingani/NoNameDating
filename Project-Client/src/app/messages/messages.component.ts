import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, PaginationClass } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
/** messages component*/
export class MessagesComponent implements OnInit {
    

  messages: Message[];
  pagination: Pagination;
  messageType = "Unread";

  /** messages ctor */
  constructor(private userService: UserService, private alertService: AlertifyService,
    private route: ActivatedRoute, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.route.data.subscribe(data => {

      this.messages = data["userData"].result;
      this.pagination = data["userData"].pagination;

    });

   
  }

  loadMessages() {
    this.userService.getMessages(this.authService.decodeToken.nameid, this.pagination.currentPage,
      this.pagination.itemsPerPage, this.messageType)
      .subscribe((response: PaginationClass<Message[]>) => {
        this.messages = response.result;
        this.pagination = response.pagination;
      }, error => {
          this.alertService.error("Request Failed");
      });

   
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  deleteMessage(id: number) {

    this.alertService.confirm("Are you sure?", () => {
      this.userService.deleteMessage(this.authService.decodeToken.nameid, id).subscribe(() => {
        this.messages.splice(this.messages.findIndex(m => m.messageID == id), 1);
        this.alertService.error("Message deleted successfully!");
      }, error => {
        this.alertService.error("Delete Message Failed");
      });
    });

  }

}
