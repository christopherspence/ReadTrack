import 'reflect-metadata';
import * as dotenv from 'dotenv';
import { DataSource } from 'typeorm';
import { User } from './entities/User';
import { Book } from './entities/Book';
import { Session } from './entities/Session';

dotenv.config();

export const AppDataSource = new DataSource({
  type: 'mssql',
  host: process.env.DB_HOST,
  port: +process.env.DB_PORT,
  username: process.env.DB_USERNAME,
  password: process.env.DB_PASSWORD,
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
