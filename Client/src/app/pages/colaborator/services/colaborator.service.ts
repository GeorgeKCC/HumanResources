import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { effect, inject, Injectable, signal, untracked } from '@angular/core';
import { Environment } from '../../../environment/environment.dev';
import { lastValueFrom } from 'rxjs';
import { GenericModel } from '../../../shared/models/generic-model/generic.model';
import { ColaboratorModel } from '../models/colaborator.model';
import { SignalrService } from '../../../shared/services/signalr.service';
import { ColaboratorCreateModel } from '../models/colaboratorCreate.model';
import { ToastService } from '../../../shared/services/toast.service';

@Injectable({
  providedIn: 'root',
})
export class ColaboratorService {
  http = inject(HttpClient);
  signalrService = inject(SignalrService);
  toastService = inject(ToastService);

  isLoading = signal<boolean>(false);
  data = signal<ColaboratorModel[]>([]);
  hasError = signal<boolean>(false);
  colaborator = signal<ColaboratorModel | null>(null);

  isUpdateLoading = signal<boolean>(false);
  updateStatus = signal<string | null>(null);

  constructor() {
    effect(() => {
      const colaborator = this.signalrService.colaboratorHub();
      if (colaborator) {
        const currentData = untracked(() => this.data());
        this.updateOrAddColaborator(currentData, colaborator);
      }
    });
    effect(() => {
      const status = this.signalrService.colaboratorUpdateStatusHub();
      this.updateStatus.set(status);
    });
    effect(() => {
      const complete = this.signalrService.colaboratorCompleteUpdateHub();
      this.updateStatus.set('');
    });
  }

  async getColaborators() {
    try {
      this.isLoading.set(true);
      const result = await lastValueFrom(
        this.http.get<GenericModel<ColaboratorModel[]>>(Environment.apiUrl + '/colaborator')
      );
      this.data.set(result.data);
      this.isLoading.set(false);
    } catch (error) {
      this.isLoading.set(false);
      this.hasError.set(true);
    }
  }

  async getColaboratorById(id: number) {
    try {
      this.isLoading.set(true);
      const result = await lastValueFrom(
        this.http.get<GenericModel<ColaboratorModel>>(Environment.apiUrl + '/colaborator/' + id)
      );
      this.colaborator.set(result.data);
      this.isLoading.set(false);
    } catch (error) {
      this.isLoading.set(false);
      this.hasError.set(true);
    }
  }

  async createColaborator(colaborator: ColaboratorCreateModel) {
    try {
      this.isLoading.set(true);
      await lastValueFrom(
        this.http.post<GenericModel<ColaboratorModel>>(
          Environment.apiUrl + '/colaborator',
          colaborator
        )
      );
      this.toastService.showToast({
        severity: 'success',
        summary: 'Create colaborator',
        message: 'Colaborator created successfully',
        code: 201,
      });
      this.isLoading.set(false);
    } catch (error) {
      this.isLoading.set(false);
      this.hasError.set(true);
      if (error instanceof HttpErrorResponse) {
        this.toastService.showToast({
          severity: 'error',
          summary: 'Error',
          message: error.status == 409 ? 'This operation already for other process.' :
                                         'Error creating colaborator. Please try again later.',
          code: error.status,
        });
      }
    }
  }

  async updateColaborator(colaborator: ColaboratorCreateModel, id: number) {
    try {
      this.isUpdateLoading.set(true);
      await lastValueFrom(
        this.http.put<GenericModel<ColaboratorModel>>(
          Environment.apiUrl + '/colaborator/' + id,
          colaborator
        )
      );
      this.isUpdateLoading.set(false);
    } catch (error) {
      this.isUpdateLoading.set(false);
    }
  }

  private updateOrAddColaborator(currentData: ColaboratorModel[], colaborator: ColaboratorModel) {
    const exists = currentData.some((c) => c.id === colaborator.id);
    if (!exists) {
      this.data.set([colaborator, ...currentData]);
    } else {
      const updated = currentData.map((c) => (c.id === colaborator.id ? colaborator : c));
      this.data.set(updated);
    }
  }
}
