import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatStatusBase'
})
export class FormatStatusBasePipe implements PipeTransform {

  transform(value: any): any {
    if (value === 1) {
      return 'Duyệt';
    } else if (value === 0) {
      return 'Chưa Duyệt';
    } else {
      return 'Không duyệt';
    }
  }

}
