import { Routes } from '@angular/router';
import { Empty } from './empty/empty';
import { ColaboratorSearchComponent } from './colaborator/search/colaborator-search.component';

export default [
    { path: 'colaborator', component: ColaboratorSearchComponent },
    { path: 'empty', component: Empty },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
