import { Entity, Column, OneToMany, JoinColumn } from 'typeorm';
import { BaseEntity } from './BaseEntity'
import { Book } from './Book';

@Entity('Users')
export class User extends BaseEntity {

    @Column({ name: 'FirstName' })
    firstName: string;

    @Column({ name: 'LastName' })
    lastName: string;

    @Column({ name: 'Email' })
    email: string;

    @Column({ name: 'Password' })
    password: string;

    @Column({ 
        name: 'ProfilePicture',
        nullable: true
     })
    profilePicture: string;

    @Column({ name: 'IsLocked' })
    isLocked: boolean = false;

    @OneToMany(() => Book, book => book.userId)
    @JoinColumn()
    books: Book[]
}
