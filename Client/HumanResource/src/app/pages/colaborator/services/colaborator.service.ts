import { GenericResponse } from "@/shared/response/generic-response.model";
import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Colaborator } from "../models/colaborator.model";
import { environment } from "../../../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class ColaboratorService {
    httpclient = inject(HttpClient);
    isLoading = signal(false);
    data = signal<Colaborator[]>([]);
    hasErrror = signal(false);

    getAllColaborators(){
        this.isLoading.set(true);
        return this.httpclient.get<GenericResponse<Colaborator[]>>(`${environment.apiUrl}/colaborator`, { withCredentials: true })
        .subscribe({
            next: (response) => {
                this.isLoading.set(false);
                this.hasErrror.set(false);
                this.data.set(response.data);
            },
            error: (err) => {
                this.isLoading.set(false);
                this.hasErrror.set(true);
                this.data.set([]);
            }
        })
    }
}