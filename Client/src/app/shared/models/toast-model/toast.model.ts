export interface ToastModel {
    severity: 'success' | 'info' | 'warn' | 'error' | '';
    summary: 'Success' | 'Info' | 'Warn' | 'Error' | '';
    message: string;
    code: number;
}