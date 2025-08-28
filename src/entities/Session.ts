import { Column, Entity, ManyToOne, OneToMany } from 'typeorm';
import { BaseEntity } from './BaseEntity';
import { Book } from './Book';

@Entity({ name: 'Sessions' })
export class Session extends BaseEntity {
    @Column({
        name: 'NumberOfPages',
        nullable: true
    })
    numberOfPages?: number;

    @Column({ name: 'Date' })
    date: Date;
    
    @Column({ name: 'DurationInSeconds' })
    durationInSeconds: number;

    @Column({
        name: 'StartPage',
        nullable: true
    })
    startPage?: number;

    @Column({
        name: 'EndPage',
        nullable: true
    })
    endPage?: number;

    @ManyToOne(() => Book, book => book.id)
    bookId: number;
}