import { BookCategory } from '../book-category.model';

export class CreateBookRequest {
    constructor(
        public name: string,
        public author: string,
        public category: BookCategory,
        public numberOfPages: number,
        public published?: Date) { }
}