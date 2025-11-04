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
import { SplitButton } from 'primeng/splitbutton';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
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
    SplitButton,
  ],
  templateUrl: './search.html',
})
export class Search implements OnInit {
  selectedColaborator!: ColaboratorModel;

  colaboratorService = inject(ColaboratorService);
  router = inject(Router);

  ngOnInit(): void {
    this.colaboratorService.getColaborators();
  }

  clear(table: Table) {
    table.clear();
  }

  view(colaborator: ColaboratorModel) {
    this.router.navigate(['/colaborator', colaborator.id]);
  }

  getActions(colaborator: ColaboratorModel) {
    let items: MenuItem[] | undefined = [];

    if (colaborator.isActive) {
      items.push({
        label: 'Deactivate',
        icon: 'pi pi-ban',
      });
    } else {
      items.push({
        label: 'Activate',
        icon: 'pi pi-check',
      });
    }

    return items;
  }
}
