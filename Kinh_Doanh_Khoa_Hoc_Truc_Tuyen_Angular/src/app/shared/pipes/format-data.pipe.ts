import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatData'
})
export class FormatDataPipe implements PipeTransform {

  transform(value: any, ...args: any): any {
   if (value !== null) {
    if (value.length > args) {
      return value.substr(0, args) + '...';
    }
   }
    return value;
  }

}
