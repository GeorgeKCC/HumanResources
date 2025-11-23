import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ColaboratorService } from '../services/colaborator.service';
import { FormBuilder, FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ColaboratorCreateModel } from '../models/colaboratorCreate.model';

@Component({
  selector: 'app-colaborator-detail',
  templateUrl: './detail.html',
  imports: [ReactiveFormsModule],
})
export class Detail implements OnInit {
  router = inject(Router);
  route = inject(ActivatedRoute);
  colaboratorService = inject(ColaboratorService);
  colaboratorForm: FormGroup

  colaboratorId?: string;

  constructor(formbuilder: FormBuilder) {
    this.colaboratorForm = formbuilder.group({
      name: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      documentType: new FormControl('', [Validators.required]),
      documentNumber: new FormControl('', [Validators.required]),
      id: new FormControl(''),
    });
  }

  async ngOnInit(): Promise<void> {
    this.colaboratorId = this.route.snapshot.paramMap.get('id')!;
    await this.colaboratorService.getColaboratorById(Number(this.colaboratorId));

    if (this.colaboratorService.hasError() == true) {
      this.router.navigate(['/colaborator']);
    }

    this.colaboratorForm.patchValue({
      name: this.colaboratorService.colaborator()?.name,
      lastName: this.colaboratorService.colaborator()?.lastName,
      email: this.colaboratorService.colaborator()?.email,
      documentType: this.colaboratorService.colaborator()?.documentType,
      documentNumber: this.colaboratorService.colaborator()?.documentNumber,
      id: this.colaboratorService.colaborator()?.id,
    });
  }

  async subtmit(){
    const colaborator: ColaboratorCreateModel = {
      name: this.colaboratorForm.value.name,
      lastName: this.colaboratorForm.value.lastName,
      email: this.colaboratorForm.value.email,
      documentType: this.colaboratorForm.value.documentType,
      documentNumber: this.colaboratorForm.value.documentNumber,
    }

    this.colaboratorService.updateColaborator(colaborator, Number(this.colaboratorForm.value.id));
  }
}
