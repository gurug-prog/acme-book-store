import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { PublisherService, PublisherDto } from '@proxy/publishers';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class PublisherComponent implements OnInit {
  publisher = { items: [], totalCount: 0 } as PagedResultDto<PublisherDto>;

  isModalOpen = false;

  form: FormGroup;

  selectedPublisher = {} as PublisherDto;

  constructor(
    public readonly list: ListService,
    private publisherService: PublisherService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    const publisherStreamCreator = query => this.publisherService.getList(query);

    this.list.hookToQuery(publisherStreamCreator).subscribe(response => {
      this.publisher = response;
    });
  }

  createPublisher() {
    this.selectedPublisher = {} as PublisherDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editPublisher(id: string) {
    this.publisherService.get(id).subscribe(publisher => {
      this.selectedPublisher = publisher;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedPublisher.name || '', Validators.required],
      address: [this.selectedPublisher.address || ''],
      contactPhone: [this.selectedPublisher.contactPhone || ''],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (this.selectedPublisher.id) {
      this.publisherService.update(this.selectedPublisher.id, this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    } else {
      this.publisherService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.publisherService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
