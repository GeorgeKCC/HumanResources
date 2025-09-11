import { Component, inject, OnInit } from '@angular/core';
import { ColaboratorService } from './services/colaborator.service';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ColaboratorCreateOrUpdate } from './models/colaboratorCreateOrUpdate';
import { ApiRequest } from '../../shared/Request/apiRequest';
import { Colaborator } from './models/Colaborator';

@Component({
  selector: 'app-colaborator',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './colaborator.component.html',
  styleUrl: './colaborator.component.css',
})
export class ColaboratorComponent implements OnInit {
  colaboratorService = inject(ColaboratorService);
  colaborators: Colaborator[] = [];

  colaboratorForm = new FormGroup({
    id: new FormControl(''),
    name: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    documentType: new FormControl('', Validators.required),
    documentNumber: new FormControl('', Validators.required),
  });

  ngOnInit(): void {
    this.getAll();
  }

  CreateOrUpdate() {
    const colaboratorId = this.colaboratorForm.value.id;

    const colaborator: ColaboratorCreateOrUpdate = {
      name: this.colaboratorForm.value.name ?? '',
      lastName: this.colaboratorForm.value.lastName ?? '',
      email: this.colaboratorForm.value.email ?? '',
      documentType: this.colaboratorForm.value.documentType ?? '',
      documentNumber: this.colaboratorForm.value.documentNumber ?? '',
    };

    if (colaboratorId) {
      this.colaboratorService.update(colaborator, Number(colaboratorId)).subscribe({
        next: (response) => {
          this.colaboratorForm.reset();
          this.getAll();
        },
        error: (exception) => {
          const error = exception as ApiRequest<Colaborator>;
          console.log(error);
        },
      });
    } else {
      this.colaboratorService.create(colaborator).subscribe({
        next: (response) => {
          this.colaboratorForm.reset();
          this.getAll();
        },
        error: (exception) => {
          const error = exception as ApiRequest<Colaborator>;
          console.log(error);
        },
      });
    }
  }

  Selected(colaborator: Colaborator) {
    this.colaboratorForm.setValue({
      id: colaborator.id.toString(),
      name: colaborator.name,
      lastName: colaborator.lastName,
      email: colaborator.email,
      documentNumber: colaborator.documentNumber,
      documentType: colaborator.documentType,
    });
  }

  getAll() {
    this.colaboratorService.getAll().subscribe({
      next: (response) => {
        this.colaborators = response.data;
      },
      error: (exception) => {
        const error = exception as ApiRequest<Colaborator>;
        console.log(error);
      },
    });
  }
}
