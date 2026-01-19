export interface GenericModel<T> {
    data: T;
    isSuccess: boolean;
    message: string;
    code: number;
}