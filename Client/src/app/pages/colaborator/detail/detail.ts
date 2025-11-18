import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ColaboratorService } from '../services/colaborator.service';

@Component({
  selector: 'app-colaborator-detail',
  templateUrl: './detail.html',
})
export class Detail implements OnInit {
  router = inject(Router);
  route = inject(ActivatedRoute);
  colaboratorService = inject(ColaboratorService);

  colaboratorId?: string;

  async ngOnInit(): Promise<void> {
    this.colaboratorId = this.route.snapshot.paramMap.get('id')!;
    await this.colaboratorService.getColaboratorById(Number(this.colaboratorId));
    if (this.colaboratorService.hasError() == true) {
      this.router.navigate(['/colaborator']);
    }
  }
}
