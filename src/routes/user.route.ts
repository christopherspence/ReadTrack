import * as express from 'express';
import { createUser, getUser, updateUser } from '../controllers/user.controller';

const router = express.Router();

router.get('/', getUser);

router.post('/', createUser);

router.put('/:id', updateUser);

// router.delete('/:id', deleteUser);

export default router;