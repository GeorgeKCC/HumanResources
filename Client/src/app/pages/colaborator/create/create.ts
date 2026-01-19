import { Component, effect, inject, model } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ColaboratorService } from '../services/colaborator.service';
import { ColaboratorCreateModel } from '../models/colaboratorCreate.model';

@Component({
  selector: 'app-colaborator-create',
  templateUrl: './create.html',
  imports: [DialogModule, ReactiveFormsModule],
})
export class Create {
  colaboratorService = inject(ColaboratorService);

  visible = model<boolean>(false);
  colaboratorForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    effect(() => {
      this.closeAndOpenModal();
    });

    this.colaboratorForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      documentType: ['', [Validators.required]],
      documentNumber: ['', [Validators.required]],
    });
  }

  private closeAndOpenModal() {
    const isVisible = this.visible();
    isVisible == true ? this.visible.set(true) : this.visible.set(false);
  }

  closeModal() {
    this.colaboratorForm.reset();
    this.visible.set(false);
  }

  onSubmit() {
    const colaboratorRequest: ColaboratorCreateModel = {
      name: this.colaboratorForm.value.name,
      lastName: this.colaboratorForm.value.lastName,
      email: this.colaboratorForm.value.email,
      documentType: this.colaboratorForm.value.documentType,
      documentNumber: this.colaboratorForm.value.documentNumber,
    }

    this.colaboratorService.createColaborator(colaboratorRequest);
    this.closeModal();
  }
}
