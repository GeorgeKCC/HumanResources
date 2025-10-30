import { Component, inject, OnInit } from '@angular/core';
import { ColaboratorService } from '../services/colaborator.service';
import { Table, TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-search',
  imports: [
    TableModule,
    ButtonModule,
    IconFieldModule,
    InputIconModule,
    MultiSelectModule,
    SelectModule,
    TagModule,
  ],
  templateUrl: './search.html',
})
export class Search implements OnInit {
  colaboratorService = inject(ColaboratorService);

  ngOnInit(): void {
    this.colaboratorService.getColaborators();
  }

  clear(table: Table) {
    table.clear();
  }
}
