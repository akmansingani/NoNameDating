import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/user';
import { PaginationClass } from '../_models/pagination';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  users: User[];
  pageno: 1;
  pagesize: 2;

  constructor(private userService: UserService, private alertService: AlertifyService) {

  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pageno, this.pagesize).subscribe((resp: PaginationClass<User[]>) => {
      this.users = resp.result;
      console.log(this.users);
    }, error => {
      this.alertService.error(error);
    });
  }

}
