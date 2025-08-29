import * as express from 'express';
import { createSession, deleteSession, getSession, getSessions, updateSession } from '../controllers/session.controller';

const router = express.Router();

router.get('/', getSessions);

router.get('/:id', getSession);

router.post('/', createSession);

router.put('/:id', updateSession);

router.delete('/:id', deleteSession);

export default router;