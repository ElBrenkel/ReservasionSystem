import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AvailableTimes } from './interfaces/availableTimes';
import { GenericList } from './interfaces/genericListResponse';
import { GenericObjectResponse } from './interfaces/genericObjectResponse';
import { GenericStatusMessage } from './interfaces/genericStatusMessage';
import { LoginPayload } from './interfaces/loginPayload';
import { LoginResponse } from './interfaces/loginResponse';
import { RegisterPayload } from './interfaces/registerPayload';
import { RoomData } from './interfaces/roomData';

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

  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders()
      .append("Accept", "application/json")
      .append("Content-Type", "application/json")
      .append("RS_USER", localStorage.getItem("user") || "")
      .append("RS_TOKEN", localStorage.getItem("token") || "");
    return headers;
  }
}

