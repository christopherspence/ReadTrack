import { BaseModel } from "./base.model";

export class Session extends BaseModel {
    constructor(
        id: number,
        public time: string,
        public numberOfPages?: number,        
        public startPage?: number,
        public endPage?: number) {
        super(id);
    }
}