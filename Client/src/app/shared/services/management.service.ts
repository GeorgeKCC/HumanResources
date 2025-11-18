import { inject, Injectable } from "@angular/core";
import { lastValueFrom } from "rxjs";
import { ManagementActiveAccessModel } from "../models/management-model/management-active-access.model";
import { Environment } from "../../environment/environment.dev";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root',
})
export class ManagementService {
    http = inject(HttpClient);

   async activeAccess(managementActiveAccessModel: ManagementActiveAccessModel): Promise<boolean> {
    const result = await lastValueFrom(this.http.post<boolean>(
      Environment.apiUrl + '/management',
      managementActiveAccessModel,
      { withCredentials: true }
    ));
    return result;
  }
}
