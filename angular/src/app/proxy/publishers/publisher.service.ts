import type { CreatePublisherDto, GetPublisherListDto, PublisherDto, UpdatePublisherDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PublisherService {
  apiName = 'Default';
  

  create = (input: CreatePublisherDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PublisherDto>({
      method: 'POST',
      url: '/api/app/publisher',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/publisher/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PublisherDto>({
      method: 'GET',
      url: `/api/app/publisher/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPublisherListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PublisherDto>>({
      method: 'GET',
      url: '/api/app/publisher',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePublisherDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/publisher/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
