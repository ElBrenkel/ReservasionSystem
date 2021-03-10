import { GenericStatusMessage } from "./genericStatusMessage";

export interface GenericObjectResponse<T> {
    status: GenericStatusMessage,
    object: T
}