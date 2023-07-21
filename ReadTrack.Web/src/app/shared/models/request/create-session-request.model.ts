export class CreateSessionRequest {
    constructor(
        public bookId: number,    
        public date: Date,
        public duration: string,
        public numberOfPages?: number,
        public startPage?: number,
        public endPage?: number) { }
}