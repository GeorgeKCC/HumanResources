export interface GenericResponse<T> {
    code: string,
    message: string,
    isSuccess: boolean,
    data: T;
}