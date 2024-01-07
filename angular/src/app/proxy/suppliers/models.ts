import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateSupplierDto {
  name: string;
  address?: string;
  contactPhone?: string;
}

export interface GetSupplierListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface SupplierDto extends AuditedEntityDto<string> {
  name?: string;
  address?: string;
  contactInformation?: string;
  minimumOrderQuantity: number;
}

export interface UpdateSupplierDto {
  name: string;
  address?: string;
  contactInformation?: string;
  minimumOrderQuantity: number;
}
