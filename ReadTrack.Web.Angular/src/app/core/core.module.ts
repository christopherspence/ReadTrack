import { NgModule, ModuleWithProviders, Optional, SkipSelf } from '@angular/core';
import { throwIfAlreadyLoaded } from './module-loaded';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReadTrackHttpInterceptor } from './interceptors';

@NgModule({
    imports: [],
    declarations: []
})
export class CoreModule {
    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        throwIfAlreadyLoaded(parentModule, 'CoreModule');
    }

    static forRoot(): ModuleWithProviders<CoreModule> {
        return {
            ngModule: CoreModule,
            providers: [
                {
                    provide: HTTP_INTERCEPTORS,
                    useClass: ReadTrackHttpInterceptor,
                    multi: true
                }
            ]
        };
    }
}
