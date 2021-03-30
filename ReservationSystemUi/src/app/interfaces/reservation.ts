export interface Reservation {
    id?: number,
    userId?: number,
    userFullName?: string,
    roomId?: number,
    rentStart: string,
    rentEnd: string,
    finalPrice?: number,
    description: string,
    status?: number
}