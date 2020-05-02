import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { TestComponent } from './test/test.component';
import { MembersListComponent } from './members/list/members-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PreventChangesGuard } from './_guards/prevent-changes';
import { ListsResolver } from './_resolver/lists.resolver';
import { MessageListResolver } from './_resolver/message-list.resolver';


const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'test', component: TestComponent },
      { path: 'members', component: MembersListComponent, resolve: { userData: MemberListResolver } },
      { path: 'members/edit', component: MemberEditComponent, resolve: { userData: MemberEditResolver }, canDeactivate: [PreventChangesGuard] },
      { path: 'members/:id', component: MemberDetailComponent, resolve: { userData: MemberDetailResolver } },
      { path: 'messages', component: MessagesComponent, resolve: { userData: MessageListResolver } },
      { path: 'lists', component: ListsComponent, resolve: { userData: ListsResolver } },
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];


@NgModule({
  imports: [ RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
