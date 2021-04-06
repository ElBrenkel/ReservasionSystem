export interface Reservation {
    id?: number,
    userId?: number,
    userFullName?: string,
    roomId?: number,
    roomName?: string,
    rentStart: string,
    rentEnd: string,
    finalPrice?: number,
    description: string,
    status?: number
}