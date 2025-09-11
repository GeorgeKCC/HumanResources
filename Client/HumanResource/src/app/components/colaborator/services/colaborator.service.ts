import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiRequest } from '../../../shared/Request/apiRequest';
import { ColaboratorCreateOrUpdate } from '../models/colaboratorCreateOrUpdate';
import { environment } from '../../../../environments/environment';
import { Colaborator } from '../models/Colaborator';

@Injectable({
  providedIn: 'root'
})
export class ColaboratorService {

  constructor() { }

  private http = inject(HttpClient);

  create(colaborator: ColaboratorCreateOrUpdate) : Observable<ApiRequest<Colaborator>>{
    return this.http.post<ApiRequest<Colaborator>>(environment.apiUrl + "/colaborator", colaborator);
  }

  update(colaborator: ColaboratorCreateOrUpdate, id: number) : Observable<ApiRequest<Colaborator>>{
    return this.http.put<ApiRequest<Colaborator>>(environment.apiUrl + "/colaborator/" + id, colaborator);
  }

  getAll() : Observable<ApiRequest<Colaborator[]>>{
    return this.http.get<ApiRequest<Colaborator[]>>(environment.apiUrl + "/colaborator");
  }
}
