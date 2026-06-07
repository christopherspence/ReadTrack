import { SegmentType } from './segment-type.model';

export class TimeSegment<T>
{
    constructor(
        public date: Date,
        public type: SegmentType,
        public value?: T) {}
}