import 'reflect-metadata';
import { DataSource } from 'typeorm';
import { User } from './entities/User';
import { Book } from './entities/Book';
import { Session } from './entities/Session';

export const AppDataSource = new DataSource({
  type: 'mssql',
  host: 'localhost',
  username: 'sa',
  password: 'r3@dtr@ck1',
  database: 'ReadTrack',
  synchronize: true,
  logging: false,
  entities: [User, Book, Session],
  migrations: [],
  subscribers: [],
  extra: {
    trustServerCertificate: true
  }
});
