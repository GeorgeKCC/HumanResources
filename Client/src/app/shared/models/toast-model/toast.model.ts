export interface ToastModel {
    severity: 'success' | 'info' | 'warn' | 'error' | '';
    summary: string;
    message: string;
    code: number;
}