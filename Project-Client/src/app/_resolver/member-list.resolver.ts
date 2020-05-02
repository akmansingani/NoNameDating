import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class MemberListResolver implements Resolve<User[]> {

  pageno: number = 1;
  pagesize: number = 5;

  constructor(private userService: UserService, private alertService: AlertifyService,
    private router: Router) {}

    resolve(): Observable<User[]> {
      return this.userService.getUsers(this.pageno, this.pagesize).pipe(
            catchError(error => {
              this.alertService.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
