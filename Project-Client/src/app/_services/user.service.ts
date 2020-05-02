import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginationClass } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';

/*const httpOptions = {
  headers: new HttpHeaders(
    {
      'Authorization': 'Bearer ' + localStorage.getItem('authtoken')
    })
}*/

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(pageno?, pagesize?,userParams?,likeParams?): Observable<PaginationClass<User[]>> {

    const result: PaginationClass<User[]> = new PaginationClass<User[]>();

    let params = new HttpParams();
    if (pageno != null && pagesize != null) {
     params = params.append("pagenumber", pageno);
      params = params.append("pagesize", pagesize);
    }

    if (userParams != null) {
      params = params.append("gender", userParams.gender);
      params = params.append("minage", userParams.minAge);
      params = params.append("maxage", userParams.maxAge);
      params = params.append("orderby", userParams.orderBy);
    }

    if (likeParams == 'likedbyuser') {
      params = params.append('likedbyuser', 'true');
    }

    if (likeParams == 'likeduser') {
      params = params.append('likeduser', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'user/', { observe: 'response', params:params })
      .pipe(
        map((response: any) => {

          result.result = response.body;
          if (response.headers.get('Pagination') != null) {
            result.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return result;

        })
     )
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'user/' + id);
  }

  updateUser(id:number,user: User) {
    return this.http.put(this.baseUrl + 'user/' + id, user);
  }

  setMainPhoto(userid: number, photoid: number) {
    return this.http.post(this.baseUrl + 'photos/setmainphoto/' + userid + '/' + photoid, {});
  }

  deletePhoto(userid: number, photoid: number) {
    return this.http.delete(this.baseUrl + 'photos/delete/' + userid + '/' + photoid, {});
  }

  likeUser(likeByUser: number, likedUser: number) {
    return this.http.post(this.baseUrl + 'user/likeuser/' + likeByUser + '/' + likedUser, {});
  }

  getMessages(id: number, pageno?, pagesize?, messagetype?) : Observable<PaginationClass<Message[]>> {

    const result: PaginationClass<Message[]> = new PaginationClass<Message[]>();

    let params = new HttpParams();

    params = params.append("messagetype", messagetype);

    if (pageno != null && pagesize != null) {
      params = params.append("pagenumber", pageno);
      params = params.append("pagesize", pagesize);
    }

    return this.http.get<Message[]>(this.baseUrl + 'messages/getmessageuser/' + id, { observe: 'response', params: params })
      .pipe(
        map((response: any) => {

          result.result = response.body;
          if (response.headers.get('Pagination') != null) {
            result.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return result;

        })
      )

  }

  getMessageThread(id: number, receiverid: number): Observable<Message[]> {

    return this.http.get<Message[]>(this.baseUrl + 'messages/getmessagethread/' + id + '/' + receiverid);

  }

  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + 'messages/createmessage/' + id, message);
  }

  deleteMessage(id: number, messageId: number) {
    return this.http.post(this.baseUrl + 'messages/deletemessage/' + id + '/' + messageId, {});
  }

  readMessage(id: number, messageId: number) {
    return this.http.post(this.baseUrl + 'messages/readmessage/' + id + '/' + messageId, {}).subscribe();
  }

}
