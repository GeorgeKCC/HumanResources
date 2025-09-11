export interface ApiRequest<T>{
    code: string,
    message: string,
    isSuccess: boolean,
    data: T
}