import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { SupplierService, SupplierDto } from '@proxy/suppliers';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class SupplierComponent implements OnInit {
  supplier = { items: [], totalCount: 0 } as PagedResultDto<SupplierDto>;

  isModalOpen = false;

  form: FormGroup;

  selectedSupplier = {} as SupplierDto;

  constructor(
    public readonly list: ListService,
    private supplierService: SupplierService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    const supplierStreamCreator = query => this.supplierService.getList(query);

    this.list.hookToQuery(supplierStreamCreator).subscribe(response => {
      this.supplier = response;
    });
  }

  createSupplier() {
    this.selectedSupplier = {} as SupplierDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editSupplier(id: string) {
    this.supplierService.get(id).subscribe(supplier => {
      this.selectedSupplier = supplier;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedSupplier.name || '', Validators.required],
      address: [this.selectedSupplier.address || ''],
      contactInformation: [this.selectedSupplier.contactInformation || ''],
      minimumOrderQuantity: [this.selectedSupplier.minimumOrderQuantity || ''],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (this.selectedSupplier.id) {
      this.supplierService.update(this.selectedSupplier.id, this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    } else {
      this.supplierService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.supplierService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
