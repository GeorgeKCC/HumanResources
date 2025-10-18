import { NgModule } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { PasswordModule } from 'primeng/password';
import { AppFloatingConfigurator } from '@/layout/component/app.floatingconfigurator';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { MessageModule } from 'primeng/message';
import { TableModule } from 'primeng/table';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SplitButtonModule } from 'primeng/splitbutton';
import { SpeedDial } from 'primeng/speeddial';
import { ToolbarModule } from 'primeng/toolbar';
import { DialogModule } from 'primeng/dialog';
import { Dialog } from 'primeng/dialog';

@NgModule({
    imports: [
        ToolbarModule,
        SpeedDial,
        MessageModule,
        ToastModule,
        ButtonModule,
        CheckboxModule,
        InputTextModule,
        PasswordModule,
        RippleModule,
        AppFloatingConfigurator,
        ProgressSpinnerModule,
        TableModule,
        IconFieldModule,
        InputIconModule,
        SplitButtonModule,
        DialogModule,
        Dialog
    ],
    exports: [
        ToolbarModule,
        SpeedDial,
        MessageModule,
        ToastModule,
        ButtonModule,
        CheckboxModule,
        InputTextModule,
        PasswordModule,
        RippleModule,
        AppFloatingConfigurator,
        ProgressSpinnerModule,
        TableModule,
        IconFieldModule,
        InputIconModule,
        SplitButtonModule,
        DialogModule,
        Dialog
    ],
    providers: [MessageService]
})
export class PrimeNgModule {}
