import { Routes } from '@angular/router';
import { Dashboard } from './pages/home/dashboard/dashboard';
import { Search } from './pages/colaborator/search/search';
import { Login } from './pages/auth/login/login';
import { Template } from './pages/layout/template/template';
import { loginGuard } from './guards/login.guard';
import { Detail } from './pages/colaborator/detail/detail';

export const routes: Routes = [
  {
    path: 'login',
    component: Login,
  },
  {
    path: '',
    component: Template,
    canActivate: [loginGuard],
    children: [
      {
        path: '',
        component: Dashboard,
      },
      {
        path: 'colaborator',
        component: Search,
      },
      {
        path: 'colaborator/:id',
        component: Detail,
      }
    ],
  },
];
