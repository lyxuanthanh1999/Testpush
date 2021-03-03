import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatStatusCourses'
})
export class FormatStatusCoursesPipe implements PipeTransform {
  transform(value: any): any {
    if (value === 1) {
      return 'Phát Hành';
    } else if (value === 2) {
      return 'Chưa Phát Hành';
    } else {
      return 'Ngừng Kinh Doanh';
    }
  }
}
