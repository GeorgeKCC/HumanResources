import { Component, input, model } from "@angular/core";
import { PrimeNgModule } from "../../../shared/primeng/primeng.module";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-create-colaborator",
  templateUrl: "./create-colaborator.component.html",
  imports: [PrimeNgModule, FormsModule]
})
export class CreateColaboratorComponent {
    visible = model<boolean>(false);

    closeModal() {
        this.visible.set(false);
    }
}