import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormatDataPipe } from './format-data.pipe';
import { ConvertPathPipe } from './convert-path.pipe';
import { FormatStatusCoursesPipe } from './format-status-courses.pipe';
import { FormatStatusBasePipe } from './format-status-base.pipe';
import { FormatOrderPipe } from './format-order.pipe';
import { FormatStatusAnnouncePipe } from './format-status-announce.pipe';
import { ConvertDatePipe } from './convert-date.pipe';

@NgModule({
    imports: [CommonModule],
    declarations: [
        FormatDataPipe,
        ConvertPathPipe,
        FormatStatusCoursesPipe,
        FormatStatusBasePipe,
        FormatOrderPipe,
        FormatStatusAnnouncePipe,
        ConvertDatePipe],
    exports: [
        FormatDataPipe,
        ConvertPathPipe,
        FormatStatusAnnouncePipe,
        FormatStatusCoursesPipe,
        FormatStatusBasePipe,
        ConvertDatePipe,
        FormatOrderPipe]
})
export class SharedPipesModule {}
