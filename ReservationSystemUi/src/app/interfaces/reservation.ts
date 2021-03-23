export interface Reservation {
    userId?: number,
    roomId?: number,
    rentStart: string,
    rentEnd: string,
    finalPrice?: number,
    description: string,
    status?: string
}