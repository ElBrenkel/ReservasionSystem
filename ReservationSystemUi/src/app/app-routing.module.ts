import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationGuard } from './guards/authentication.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
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
    path: "room/:id",
    component: RoomViewComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: "**",
    redirectTo: "login"
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
