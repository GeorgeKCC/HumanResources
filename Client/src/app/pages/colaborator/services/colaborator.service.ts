import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Environment } from '../../../environment/environment.dev';
import { lastValueFrom } from 'rxjs';
import { GenericModel } from '../../../shared/models/generic-model/generic.model';
import { ColaboratorModel } from '../models/colaborator.model';

@Injectable({
  providedIn: 'root',
})
export class ColaboratorService {
  http = inject(HttpClient);
  isLoading = signal<boolean>(false);
  data = signal<ColaboratorModel[]>([]);
  hasError = signal<boolean>(false);

  async getColaborators() {
    try {
      this.isLoading.set(true);
      const result = await lastValueFrom(
        this.http.get<GenericModel<ColaboratorModel[]>>(Environment.apiUrl + '/colaborator', { withCredentials: true })
      );
      this.data.set(result.data);
      this.isLoading.set(false);
    } catch (error) {
      this.isLoading.set(false);
      this.hasError.set(true);
    }
  }
}
