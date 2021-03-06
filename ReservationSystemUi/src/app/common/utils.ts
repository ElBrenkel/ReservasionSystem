import { MatSnackBar } from "@angular/material/snack-bar";
import { RoomData } from "../interfaces/roomData";
import { ReservationStatusSnackbarComponent } from "../reservation-status-snackbar/reservation-status-snackbar.component";

export class Utils {
    public static dayStrings = {
        1: "Sunday",
        2: "Monday",
        3: "Tuesday",
        4: "Wednesday",
        5: "Thursday",
        6: "Friday",
        7: "Saturday"
    };

    public static calcPicNumber(seed: string): number {
        const number = seed.split("").map(x => x.charCodeAt(0)).reduce((x, y) => x + y);
        return (number % 6) + 1;
    }

    public static roomAddress(room: RoomData): string {
        return `${room.street} ${room.buildingNumber} ${room.city}`;
    }

    public static getDayString(day: number): string {
        return this.dayStrings[day];
    }

    public static getHour(time: number): string {
        const hours = `${Math.floor(time / 60)}`.padStart(2, "0");
        const minutes = `${time % 60}`.padStart(2, "0");
        return `${hours}:${minutes}`;
    }

    public static getHourFromDate(time: Date): string {
        const hours = time.getHours();
        const minutes = time.getMinutes();
        return this.getHour(hours * 60 + minutes);
    }

    public static addMinutes(date: Date, minutes: number): Date {
        return new Date(date.getTime() + minutes * 60000);
    }

    public static getMinutesFromTimeString(timeString: string) {
        const parts = timeString.split(":");
        const hours = Number.parseInt(parts[0]);
        const minutes = Number.parseInt(parts[1]);
        return hours * 60 + minutes;
    }

    public static openSnackbar(snackBar: MatSnackBar, success: boolean, message: string): void {
        const data = { success, message };
        const panelClass = success ? ['mat-toolbar', 'mat-primary'] : ['mat-toolbar', 'mat-warn'];
        const duration = 5000;
        snackBar.openFromComponent(ReservationStatusSnackbarComponent, { data, panelClass, duration });
    }
}