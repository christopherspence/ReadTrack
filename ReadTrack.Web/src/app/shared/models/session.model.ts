import { BaseModel } from './base.model';

export class Session extends BaseModel {
    constructor(
        id: number,
        public date: Date,
        public duration: string,
        public numberOfPages?: number,        
        public startPage?: number,
        public endPage?: number) {
        super(id);
    }
}