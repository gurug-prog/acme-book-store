import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PublisherRoutingModule } from './publisher-routing.module';
import { PublisherComponent } from './publisher.component';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [PublisherComponent],
  imports: [SharedModule, PublisherRoutingModule, NgbDatepickerModule],
})
export class PublisherModule {}
