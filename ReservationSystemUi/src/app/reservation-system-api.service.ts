import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginPayload } from './interfaces/loginPayload';
import { LoginResponse } from './interfaces/loginResponse';

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
}
