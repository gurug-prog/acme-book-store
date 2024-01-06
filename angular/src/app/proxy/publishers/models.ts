import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePublisherDto {
  name: string;
  address?: string;
  contactPhone?: string;
}

export interface GetPublisherListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface PublisherDto extends AuditedEntityDto<string> {
  name?: string;
  address?: string;
  contactPhone?: string;
}

export interface UpdatePublisherDto {
  name: string;
  address?: string;
  contactPhone?: string;
}
