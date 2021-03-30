import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './login/login.component';
import { AppMaterialModule } from './material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { TextInputComponent } from './text-input/text-input.component';
import { TopNavComponent } from './top-nav/top-nav.component';
import { SearchResultsComponent } from './search-results/search-results.component';
import { RoomCardComponent } from './room-card/room-card.component';
import { RoomViewComponent } from './room-view/room-view.component';
import { RoomViewHeaderComponent } from './room-view-header/room-view-header.component';
import { RoomViewWorkingHoursComponent } from './room-view-working-hours/room-view-working-hours.component';
import { RoomEditWorkingHoursComponent } from './room-edit-working-hours/room-edit-working-hours.component';
import { RoomAddReservationComponent } from './room-add-reservation/room-add-reservation.component';
import { ReservationStatusSnackbarComponent } from './reservation-status-snackbar/reservation-status-snackbar.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { ProfileViewComponent } from './profile-view/profile-view.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserFieldComponent } from './user-field/user-field.component';
import { NewRoomComponent } from './new-room/new-room.component';
import { ReservationCalendarComponent } from './reservation-calendar/reservation-calendar.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { ReservationCalendarHeaderComponent } from './reservation-calendar-header/reservation-calendar-header.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    TextInputComponent,
    TopNavComponent,
    SearchResultsComponent,
    RoomCardComponent,
    RoomViewComponent,
    RoomViewHeaderComponent,
    RoomViewWorkingHoursComponent,
    RoomEditWorkingHoursComponent,
    RoomAddReservationComponent,
    ReservationStatusSnackbarComponent,
    ConfirmDialogComponent,
    ProfileViewComponent,
    EditProfileComponent,
    ChangePasswordComponent,
    UserFieldComponent,
    NewRoomComponent,
    ReservationCalendarComponent,
    ReservationCalendarHeaderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AppMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxMaterialTimepickerModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
