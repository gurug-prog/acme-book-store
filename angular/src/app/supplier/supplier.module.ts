import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SupplierRoutingModule } from './supplier-routing.module';
import { SupplierComponent } from './supplier.component';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [SupplierComponent],
  imports: [SharedModule, SupplierRoutingModule, NgbDatepickerModule],
})
export class SupplierModule {}
