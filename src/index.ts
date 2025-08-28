import { createDatabase } from 'typeorm-extension';
import { AppDataSource } from './data-source';
import { User } from './entities/User';

(async () => {
  await createDatabase({ ifNotExist: true });
  console.log('Created the database...');
  AppDataSource.initialize()
    .then(async () => {    
      console.log(
        'Here you can setup and run express / fastify / any other framework.'
      );
    })
    .catch((error) => console.log(error));
})();
