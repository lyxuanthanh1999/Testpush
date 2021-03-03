import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { SharedPipesModule } from '../shared';
import { ConvertPathPipe } from '../shared/pipes/convert-path.pipe';
import { FormatDataPipe } from '../shared/pipes/format-data.pipe';
import { FormatOrderPipe } from '../shared/pipes/format-order.pipe';
import { FormatStatusBasePipe } from '../shared/pipes/format-status-base.pipe';
import { FormatStatusCoursesPipe } from '../shared/pipes/format-status-courses.pipe';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';

@NgModule({
    imports: [CommonModule,
         LayoutRoutingModule,
          TranslateModule,
           NgbDropdownModule,
           SharedPipesModule,
        ],
    declarations: [LayoutComponent,
         SidebarComponent,
          HeaderComponent,

        ]
})
export class LayoutModule {}
