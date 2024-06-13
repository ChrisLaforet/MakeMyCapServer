import { MutableRefObject } from 'react';
import { CapModelContentHandle } from './CapModel';
import { CapStyleContentHandle } from './CapStyle';
import { ImagePlacementContentHandle } from './ImagePlacement';

export class CapModelContentRef implements MutableRefObject<CapModelContentHandle | null> {
    public current: CapModelContentHandle | null = null;
}

export class CapStyleContentRef implements MutableRefObject<CapStyleContentHandle | null> {
    public current: CapStyleContentHandle | null = null;
}

export class ImagePlacementContentRef implements MutableRefObject<ImagePlacementContentHandle | null> {
    public current: ImagePlacementContentHandle | null = null;
}
