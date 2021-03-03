import { GenericStatusMessage } from "./genericStatusMessage";

export interface GenericList<T> {
    status: GenericStatusMessage,
    items: T[],
    totalCount: number
}