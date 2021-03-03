import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatStatusAnnounce'
})
export class FormatStatusAnnouncePipe implements PipeTransform {

  transform(value: any): string {
    if (!value) {
      return 'Chưa đọc';
    }  else {
      return 'Đã đọc';
    }
  }

}
