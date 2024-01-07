import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';
import type { BookType } from './book-type.enum';

export interface AuthorLookupDto extends EntityDto<string> {
  name?: string;
}

export interface BookDto extends AuditedEntityDto<string> {
  name?: string;
  type: BookType;
  publishDate?: string;
  price: number;
  authorId?: string;
  authorName?: string;
  publisherId?: string;
  publisherName?: string;
  coverImage?: string;
}

export interface CreateUpdateBookDto {
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
  authorId?: string;
  publisherId?: string;
  coverImage?: string;
}

export interface PublisherLookupDto extends EntityDto<string> {
  name?: string;
}
