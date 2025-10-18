import { Component, inject, signal, Signal } from '@angular/core';
import { ColaboratorService } from '../services/colaborator.service';
import { PrimeNgModule } from '@/shared/primeng/primeng.module';
import { Table } from 'primeng/table';
import { Colaborator } from '../models/colaborator.model';
import { MenuItem } from 'primeng/api';
import { CreateColaboratorComponent } from '../create/create-colaborator.component';

@Component({
    selector: 'app-colaborator-search',
    templateUrl: './colaborator-search.component.html',
    imports: [PrimeNgModule, CreateColaboratorComponent]
})
export class ColaboratorSearchComponent {
    colaboratorService = inject(ColaboratorService);
    searchValue: string | undefined;
    visible = signal(false)

    ngOnInit() {
        this.colaboratorService.getAllColaborators();
    }

    addUser() {
        this.visible.set(true);
        console.log('Add user');
    }

    viewDetail(colaborator: Colaborator) {
        console.log('View detail', colaborator);
    }

    deleteUser(colaborator?: Colaborator) {
        console.log('Delete user', colaborator);
    }

    refresh() {
        this.colaboratorService.getAllColaborators();
    }

    clear(table: Table) {
        table.clear();
        this.searchValue = '';
    }

    getMenuItems(colaborator: Colaborator): MenuItem[] {
        return [
            { 
                label: 'Dar acceso a plataforma', 
                icon: 'pi pi-plus', 
                command: () => {
                    this.addUser();
                }
            },
            { 
                label: 'Inactivar colaborador', 
                icon: 'pi pi-trash', 
                command: () => {
                    this.deleteUser(colaborator);
                }
            }
        ];
    }
}