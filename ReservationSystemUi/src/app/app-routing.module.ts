import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationGuard } from './guards/authentication.guard';
import { RoomOwnerGuard } from './guards/roomOwner.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { NewRoomComponent } from './new-room/new-room.component';
import { ProfileViewComponent } from './profile-view/profile-view.component';
import { RegisterComponent } from './register/register.component';
import { ReservationCalendarComponent } from './reservation-calendar/reservation-calendar.component';
import { RoomViewComponent } from './room-view/room-view.component';

const routes: Routes = [
  {
    path: "register",
    component: RegisterComponent
  }, {
    path: "login",
    component: LoginComponent
  }, {
    path: "",
    component: HomeComponent,
    canActivate: [AuthenticationGuard]
  }, {
    path: "room",
    component: NewRoomComponent,
    canActivate: [AuthenticationGuard, RoomOwnerGuard]
  }, {
    path: "room/:id",
    component: RoomViewComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: "profile",
    component: ProfileViewComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: "**",
    redirectTo: "",
    canActivate: [AuthenticationGuard]
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
