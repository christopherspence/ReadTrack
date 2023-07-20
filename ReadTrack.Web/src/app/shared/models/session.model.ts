import { BaseModel } from './base.model';
import { TimeSpan } from './time-span.model';

export class Session extends BaseModel {
    constructor(
        id: number,
        public date: Date,
        public duration: TimeSpan,
        public numberOfPages?: number,        
        public startPage?: number,
        public endPage?: number) {
        super(id);
    }
}