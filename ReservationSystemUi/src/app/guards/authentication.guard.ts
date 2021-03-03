import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { ReservationSystemApiService } from "../reservation-system-api.service";

@Injectable({
    providedIn: "root"
})
export class AuthenticationGuard implements CanActivate {
    constructor(private api: ReservationSystemApiService, private router: Router) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        return this.api.isLoggedIn().then(r => {
            if (!r) {
                window.location.href = "/login";
            }

            return r;
        }).catch(r => {
            window.location.href = "/login";
            return false;
        });
    }
}