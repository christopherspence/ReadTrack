import { Column, Entity, ManyToOne, OneToMany } from 'typeorm';
import { BaseEntity } from './BaseEntity';
import { BookCategory } from '../models/BookCategory';
import { User } from './User';
import { Session } from './Session';

@Entity({ name: 'Books' })
export class Book extends BaseEntity {
    @Column({ name: 'Name' })
    name: string;
    
    @Column({ name: 'Author' })
    author: string;
    
    @Column({ name: 'Category' })
    category: string;

    @Column({
        name: 'Published',
        nullable: true
    })
    published?: Date;

    @Column({ name: 'NumberOfPages' })
    numberOfPages: number;

    @Column({ name: 'Finished' })
    finished: boolean = false;

    @ManyToOne(() => User, user => user.id)
    userId: number;

    @OneToMany(() => Session, session => session.bookId)
    sessions: Session[];
}
