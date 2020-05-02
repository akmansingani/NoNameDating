import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';



@Injectable()
export class MessageListResolver implements Resolve<Message[]> {

  pageno: number = 1;
  pagesize: number = 5;
  messagetype: string = "Unread";

  constructor(private userService: UserService, private alertService: AlertifyService,
    private router: Router, private authService: AuthService) { }

  resolve(): Observable<Message[]> {
    return this.userService.getMessages(this.authService.decodeToken.nameid,this.pageno, this.pagesize, this.messagetype).pipe(
            catchError(error => {
              this.alertService.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
