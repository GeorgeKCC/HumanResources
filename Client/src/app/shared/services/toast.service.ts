import { effect, inject, Injectable, signal } from '@angular/core';
import { ToastModel } from '../models/toast-model/toast.model';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private messageService = inject(MessageService);
  toastModels = signal<ToastModel>({
    severity: '',
    summary: '',
    message: '',
    code: 0
  });

  constructor() {
    effect(() => {
        const toastModel = this.toastModels();
        if(toastModel.code != 0){
          this.showToast(toastModel);
        }
    });
  }

  showToast(toastModel: ToastModel) {
    this.messageService.add({
      severity: toastModel.severity,
      summary: toastModel.summary,
      detail: toastModel.message,
      life: 3000,
    });
  }
}
