import { Reservation } from "./reservation";
import { WorkingHours } from "./workingHours";

export interface RoomData {
    id: number,
    name: string,
    size: number,
    maxNumberOfPeople: number,
    country: string,
    city: string,
    street: string,
    buildingNumber: number,
    lat: number,
    lon: number,
    isActive: boolean,
    isOwner: boolean,
    workingHours: WorkingHours[],
    reservations: Reservation[]
}