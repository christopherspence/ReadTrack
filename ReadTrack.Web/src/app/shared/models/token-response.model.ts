import { User } from './user.model';

export class TokenResponse {
    constructor(
        public type: string,
        public token: string,
        public user: User,
        public issued: Date,
        public expires: Date) { }
}