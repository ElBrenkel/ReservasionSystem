
export interface UserRole {
    name: string;
    id: number;
}

export class DefaultRoles {
    public static get COACH(): UserRole {
        return {
            name: "Coach",
            id: 2
        }
    }

    public static get ROOM_OWNER(): UserRole {
        return {
            name: "Room owner",
            id: 1
        }
    }

    public static get ALL_ROLES(): UserRole[] {
        return [this.COACH, this.ROOM_OWNER];
    }
}