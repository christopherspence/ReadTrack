import { BookCategory } from "./book-category.model";

export class Book {
    constructor(
        public name: string,
        public author: string,
        public category: BookCategory,
        public numberOfPages: number,
        public finished: boolean,
        public published?: Date) {}
}