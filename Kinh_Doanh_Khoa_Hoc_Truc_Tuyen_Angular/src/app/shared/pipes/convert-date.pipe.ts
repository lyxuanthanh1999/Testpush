import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';


@Pipe({
  name: 'convertDate'
})
export class ConvertDatePipe implements PipeTransform {

  transform(value: any): string {
    moment.locale('vi');
    return moment(value).fromNow();
  }

}
