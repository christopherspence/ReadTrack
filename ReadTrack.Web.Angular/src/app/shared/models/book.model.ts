import { Session } from 'inspector';
import { BaseModel } from './base.model';
import { BookCategory } from './book-category.model';

export class Book extends BaseModel {
    constructor(
        id: number,
        public name: string,
        public author: string,
        public category: BookCategory,
        public numberOfPages: number,
        public finished: boolean,
        public sessions: Array<Session>,
        public published?: Date) {
            super(id);
        }
}