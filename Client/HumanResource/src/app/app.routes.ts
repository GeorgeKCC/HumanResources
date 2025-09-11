import { Routes } from '@angular/router';
import { ColaboratorComponent } from './components/colaborator/colaborator.component';
import { ManagementComponent } from './components/management/management.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './shared/guards/authGuard/auth.guard';

export const routes: Routes = [
    {
        path: '',
        component: LoginComponent,
        title: 'Login'
    },
    {
        path: 'Colaborator',
        component: ColaboratorComponent,
        title: 'Colaborador',
        canActivate: [authGuard]
    },
    {
        path: 'management',
        component: ManagementComponent,
        title: 'Administracion',
        canActivate: [authGuard]
    }
];
