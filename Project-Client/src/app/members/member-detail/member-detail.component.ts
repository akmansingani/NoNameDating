import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, ImageItem } from '@ngx-gallery/core';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
/** member-detail component*/
export class MemberDetailComponent implements OnInit {

  @ViewChild('membertab', { static: true }) membertab: TabsetComponent;
  user: User;
  images: GalleryItem[];
  

  /** member-detail ctor */
  constructor(private userService: UserService, private alertService: AlertifyService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['userData'];
    });

    this.route.queryParams.subscribe(params => {
      var queryTab = params['tab'];
      this.membertab.tabs[queryTab > 0 ? queryTab : 0].active = true;
    });

    if (this.user) {
      if (this.user.photourl === null) {
        this.user.photourl = "/assets/dummyImage.png";
      }
    }

    this.images = this.getImages();

  

  }

  getImages() {
    const imageUrls = [];
    for (let i = 0; i < this.user.photos.length; i++) {
      var objImage = new ImageItem({ src: this.user.photos[i].url, thumb: this.user.photos[i].url });
      imageUrls.push(objImage);
    }

    return imageUrls;

  }

  selectTab(tabid: number) {
    this.membertab.tabs[tabid].active = true;
  }


  /* loadUser() {
     this.userService.getUser(+this.route.snapshot.params['id']).subscribe((resp: User) => {
       this.user = resp;
     }, error => {
         this.alertService.error(error);
     });
   }*/

}

// 
