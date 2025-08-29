import { createDatabase } from 'typeorm-extension';
import * as dotenv from 'dotenv';
import * as express from 'express';
import { AppDataSource } from './data-source';
import authRoutes from './routes/auth.route';
import bookRoutes from './routes/book.route';
import sessionRoutes from './routes/session.route';
import userRoutes from './routes/user.route';

dotenv.config();

(async () => {
  await createDatabase({ ifNotExist: true });
  console.log('Created the database...');
  AppDataSource.initialize()
    .then(async () => {    
      const app = express();
      app.use(express.json());

      const port = process.env.PORT || 3000;

      app.use('/api/auth', authRoutes);
      app.use('/api/book', bookRoutes);
      app.use('/api/session', sessionRoutes);
      app.use('/api/user', userRoutes);

      app.listen(port, () => {
        console.log(`app is running at http://localhost:${port}`);
      });
    })
    .catch((error) => console.log(error));
})();
