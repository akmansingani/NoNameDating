import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from '../../_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { error } from 'protractor';

@Component({
    selector: 'app-photo-edit',
    templateUrl: './photo-edit.component.html',
    styleUrls: ['./photo-edit.component.css']
})
/** photo-edit component*/
export class PhotoEditComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getMemberPhoto = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean = false;
  baseUrl = environment.apiUrl;
  mainPhoto: Photo;

  /** photo-edit ctor */
  constructor(private authservice: AuthService, private userservice: UserService, private alertservice: AlertifyService) {
   
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

 

  ngOnInit() {

   // console.log(this.photos);

    this.initialiazeUpload();

  }

  initialiazeUpload() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'photos/addphoto/' + this.authservice.decodeToken.nameid,
      authToken: 'Bearer ' + localStorage.getItem('authtoken'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,

    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          createdDate: res.createdDate,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.authservice.currentUser.photourl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authservice.currentUser));

          this.authservice.setDecodeToken();

          this.getMemberPhoto.emit(photo.url);
        }
      }
    };
  }

  setMainPhoto(photo: Photo) {

    //console.log(photo);

    this.userservice.setMainPhoto(this.authservice.decodeToken.nameid, photo.id).subscribe(() => {

      this.mainPhoto = this.photos.filter(p => p.isMain === true)[0];
      this.mainPhoto.isMain = false;
      photo.isMain = true;

      this.authservice.currentUser.photourl = photo.url;

      localStorage.setItem('user', JSON.stringify(this.authservice.currentUser));

      this.authservice.setDecodeToken();

      this.getMemberPhoto.emit(photo.url);

      this.alertservice.success("Photo set successfully!");
    }, error => {
        this.alertservice.error("Error setting main photo");
    });

  }

  deletePhoto(photo: Photo) {

    this.alertservice.confirm('Are you sure you want to delete photo ?', () => {

      this.userservice.deletePhoto(this.authservice.decodeToken.nameid, photo.id).subscribe(() => {

        this.alertservice.success("Photo deleted successfully!");

        console.log(this.photos);

        this.photos.splice(this.photos.findIndex(p => p.id === photo.id), 1);

        console.log(this.photos);

        
      }, error => {
          this.alertservice.error(error);
      });

    });

   
  }

}
