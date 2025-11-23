import { Component, inject, OnInit } from '@angular/core';
import { ColaboratorService } from '../services/colaborator.service';
import { Table, TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { ColaboratorModel } from '../models/colaborator.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToolbarModule } from 'primeng/toolbar';
import { Router } from '@angular/router';
import { Create } from "../create/create";
import { ManagementService } from '../../../shared/services/management.service';
import { ManagementActiveAccessModel } from '../../../shared/models/management-model/management-active-access.model';

@Component({
  selector: 'app-colaborator-search',
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    IconFieldModule,
    InputIconModule,
    MultiSelectModule,
    SelectModule,
    TagModule,
    ToolbarModule,
    Create
],
  templateUrl: './search.html',
})
export class Search implements OnInit {
  selectedColaborator!: ColaboratorModel;

  colaboratorService = inject(ColaboratorService);
  router = inject(Router);
  managementService = inject(ManagementService);

  visible = false;

  ngOnInit(): void {
    this.colaboratorService.getColaborators();
  }

  clear(table: Table) {
    table.clear();
  }

  view(colaborator: ColaboratorModel) {
    this.router.navigate(['/colaborator', colaborator.id]);
  }

  create(){
    this.visible = true;
  }

  async activeAccess(colaborator: ColaboratorModel){
   const activeAccess: ManagementActiveAccessModel = {
      colaboratorId: colaborator.id,
   };
   await this.managementService.activeAccess(activeAccess);
  }

  async deactivateAccess(colaborator: ColaboratorModel){
    const activeAccess: ManagementActiveAccessModel = {
       colaboratorId: colaborator.id,
    };
    await this.managementService.deactivateAccess(activeAccess);
   }
}
