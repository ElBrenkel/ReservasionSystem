import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AvailableTimes } from './interfaces/availableTimes';
import { ChangePassword } from './interfaces/changePassword';
import { GenericList } from './interfaces/genericListResponse';
import { GenericObjectResponse } from './interfaces/genericObjectResponse';
import { GenericStatusMessage } from './interfaces/genericStatusMessage';
import { LoginPayload } from './interfaces/loginPayload';
import { LoginResponse } from './interfaces/loginResponse';
import { RegisterPayload } from './interfaces/registerPayload';
import { Reservation } from './interfaces/reservation';
import { RoomData } from './interfaces/roomData';
import { User } from './interfaces/user';
import { WorkingHours } from './interfaces/workingHours';

@Injectable({
  providedIn: 'root'
})
export class ReservationSystemApiService {

  constructor(private http: HttpClient) { }

  public login(payload: LoginPayload): Promise<LoginResponse> {
    return this.http.post("https://localhost:5001/api/user/login", payload)
      .toPromise()
      .then((r) => {
        return r as LoginResponse;
      })
      .catch((r) => {
        console.log(r);
        return null;
      });
  }

  public register(payload: RegisterPayload, userRole: number): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/user/${userRole}`, payload)
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public isLoggedIn(): Promise<boolean> {
    return this.http.get(`https://localhost:5001/api/user/ping`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return true;
      })
      .catch((r) => {
        console.log(r);
        return false;
      });
  }

  public isRoomOwner(): Promise<boolean> {
    return this.http.get(`https://localhost:5001/api/user/isRoomOwner`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return true;
      })
      .catch((r) => {
        console.log(r);
        return false;
      });
  }

  public searchRooms(searchQuery: string): Promise<GenericList<RoomData>> {
    return this.http.get(`https://localhost:5001/api/room/search?city=${searchQuery}&name=${searchQuery}&take=1000`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericList<RoomData>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericList<RoomData>;
      });
  }

  public getReservations(roomId: number): Promise<GenericList<Reservation>> {
    return this.http.get(`https://localhost:5001/api/room/${roomId}/request/list?take=1000`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericList<Reservation>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericList<Reservation>;
      });
  }

  public getUserReservations(): Promise<GenericList<Reservation>> {
    return this.http.get(`https://localhost:5001/api/user/reservations`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericList<Reservation>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericList<Reservation>;
      });
  }

  public getOwnedRooms(): Promise<GenericList<RoomData>> {
    return this.http.get(`https://localhost:5001/api/user/rooms`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericList<RoomData>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericList<RoomData>;
      });
  }

  public getRoom(roomId: string): Promise<GenericObjectResponse<RoomData>> {
    return this.http.get(`https://localhost:5001/api/room/${roomId}?expand=true`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<RoomData>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<RoomData>;
      });
  }

  public getAvailableTimes(roomId: number, date: Date, duration: number): Promise<GenericList<AvailableTimes>> {
    console.log({ date });
    return this.http.get(`https://localhost:5001/api/room/${roomId}/availableTimes?startDate=${date.toISOString()}&duration=${duration}`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericList<AvailableTimes>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericList<AvailableTimes>;
      });
  }

  public requestReservation(roomId: number, reservation: Reservation): Promise<GenericObjectResponse<Reservation>> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/request`, reservation, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<Reservation>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<Reservation>;
      });
  }

  public editRoomData(roomData: RoomData): Promise<GenericObjectResponse<RoomData>> {
    return this.http.patch(`https://localhost:5001/api/room/${roomData.id}`, roomData, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<RoomData>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<RoomData>;
      });
  }

  public addRoomData(roomData: RoomData): Promise<GenericObjectResponse<RoomData>> {
    return this.http.post(`https://localhost:5001/api/room`, roomData, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<RoomData>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<RoomData>;
      });
  }

  public acceptReservation(roomId: number, requestId: number): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/request/${requestId}/accept`, null, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public rejectReservation(roomId: number, requestId: number): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/request/${requestId}/reject`, null, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public deactivateRoom(roomId: number, force: boolean): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/deactivate`, force, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public activateRoom(roomId: number): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/activate`, null, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public logout(): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/user/logout`, null, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public getUserData(): Promise<GenericObjectResponse<User>> {
    return this.http.get(`https://localhost:5001/api/user`, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<User>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<User>;
      });
  }

  public changeUserData(user: User): Promise<GenericObjectResponse<User>> {
    return this.http.patch(`https://localhost:5001/api/user`, user, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<User>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<User>;
      });
  }

  public changePassword(payload: ChangePassword): Promise<GenericStatusMessage> {
    return this.http.post(`https://localhost:5001/api/user/changePassword`, payload, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericStatusMessage;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericStatusMessage;
      });
  }

  public editWorkingHours(roomId: number, payload: WorkingHours[]): Promise<GenericObjectResponse<WorkingHours[]>> {
    return this.http.post(`https://localhost:5001/api/room/${roomId}/workingHours`, payload, { headers: this.getHeaders() })
      .toPromise()
      .then((r) => {
        return r as GenericObjectResponse<WorkingHours[]>;
      })
      .catch((r) => {
        console.log(r);
        return r.error as GenericObjectResponse<WorkingHours[]>;
      });
  }

  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders()
      .append("Accept", "application/json")
      .append("Content-Type", "application/json")
      .append("RS_USER", localStorage.getItem("user") || "")
      .append("RS_TOKEN", localStorage.getItem("token") || "");
    return headers;
  }
}

