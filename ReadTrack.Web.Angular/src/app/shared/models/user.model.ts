import { BaseModel } from './base.model';

export class User extends BaseModel {
    constructor(
        id: number,
        public firstName: string,
        public lastName: string,
        public email: string,
        public password: string,
        public profilePicture: string,
        public isLocked: boolean) { 
        super(id);
    }
}