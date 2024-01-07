import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {
  BookService,
  BookDto,
  bookTypeOptions,
  AuthorLookupDto,
  PublisherLookupDto,
} from '@proxy/books';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FileServiceService } from '../shared/file-service.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  form: FormGroup;

  selectedBook = {} as BookDto;

  authors$: Observable<AuthorLookupDto[]>;
  publishers$: Observable<PublisherLookupDto[]>;

  bookTypes = bookTypeOptions;

  isModalOpen = false;
  coverImage: string;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private fileService: FileServiceService
  ) {
    this.authors$ = bookService.getAuthorLookup().pipe(map(r => r.items));
    this.publishers$ = bookService.getPublisherLookup().pipe(map(r => r.items));
  }

  ngOnInit() {
    const bookStreamCreator = query => this.bookService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe(response => {
      this.book = response;
    });
  }

  async onCoverChange(event) {
    const file = event.target.files[0];
    this.coverImage = await this.fileService.fileToDataUrl(file);
  }

  createBook() {
    this.selectedBook = {} as BookDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editBook(id: string) {
    this.bookService.get(id).subscribe(async book => {
      this.selectedBook = book;
      this.coverImage = await this.fileService.base64ToDataURL(
        'application/image',
        book.coverImage
      );
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      authorId: [this.selectedBook.authorId || null, Validators.required],
      publisherId: [this.selectedBook.publisherId || null, Validators.required],
      name: [this.selectedBook.name || null, Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
        Validators.required,
      ],
      price: [this.selectedBook.price || null, Validators.required],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const data = {
      ...this.form.value,
    };

    if (this.coverImage?.length) {
      data.coverImage = this.fileService.dataUrlToBase64(this.coverImage);
    }

    const request = this.selectedBook.id
      ? this.bookService.update(this.selectedBook.id, data)
      : this.bookService.create(data);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.bookService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
