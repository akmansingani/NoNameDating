import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginationClass } from '../../_models/pagination';

@Component({
    selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
/** members component*/
export class MembersListComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  userParams: any = {};
  genderList = [{ value: '', display: 'All' },{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

  constructor(private userService: UserService, private alertService: AlertifyService, private route: ActivatedRoute) {

  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data["userData"].result;
      this.pagination = data["userData"].pagination;
    });

    this.resetFilter(false);
  }

  resetFilter(varUser) {
    this.userParams.gender = "";
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 99;
    if (varUser) {
      this.loadUsers();
    }
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((resp: PaginationClass<User[]>) => {
        this.users = resp.result;
        this.pagination = resp.pagination;
    }, error => {
      this.alertService.error("Request Failed");
    });
  }

}
