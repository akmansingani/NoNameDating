import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { User } from '../_models/user';
import { PaginationClass, Pagination } from '../_models/pagination';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
/** lists component*/
export class ListsComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  linkParams: String;

  /** lists ctor */
  constructor(private alertService: AlertifyService, private authService: AuthService
    , private userService: UserService, private route: ActivatedRoute) {

  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data["userData"].result;
      this.pagination = data["userData"].pagination;
    });
    this.linkParams = "likedbyuser";
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.linkParams)
      .subscribe((resp: PaginationClass<User[]>) => {
        this.users = resp.result;
        this.pagination = resp.pagination;
      }, error => {
        this.alertService.error("Request Failed");
      });
  }
}
