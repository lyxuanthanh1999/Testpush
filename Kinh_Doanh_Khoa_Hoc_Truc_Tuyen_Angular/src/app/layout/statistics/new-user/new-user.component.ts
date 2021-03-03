import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { StatisticsService } from '../../../shared/services';
import { Statistic } from '../../../shared/models/statistic.model';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public blockedPanel = false;
  public blockedPanelChart = false;
  /**
   * Date
   */
  public vi: any;
  public fromDate: any;
  public toDate: any;
  public lstDate = [];
  public lstCount = [];
  public isChangeChart = null;
  public isChange = false;
  public chooseType =
  [
    {value: null, label: 'Chọn kiểu lọc'},
    {value: 1, label: '1 tuần trước'},
    {value: 2, label: '1 tháng trước'},
    {value: 3, label: 'Phạm vi'}
  ];
  public isChooseType = null;
  public chooseChart =
  [
    {value: null, label: 'Chọn biểu đồ'},
    {value: true, label: 'Bar'},
    {value: false, label: 'Line'}
  ];
  public data: any;
   /**
   * Table
   */
  public totalRecords = 0;
  public items: Statistic[];
  public showDetails = false;
  public totalDetailsRecords: number;
  constructor(private statisticsService: StatisticsService,
    private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.vi = {
      firstDayOfWeek: 0,
      dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
      dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
      monthNames: ['January', 'February', 'March', 'April', 'May',
       'June', 'July', 'August', 'September', 'October', 'November', 'December'],
      monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      today: 'Today',
      clear: 'Clear'
  };
  }
  onChangeType() {
    this.items = null;
    this.totalRecords = 0;
    this.fromDate = null;
    this.toDate = null;
    this.isChangeChart = null;
    if (this.isChooseType === 1) {
      this.isChange = true;
    } else if (this.isChooseType === 2) {
      this.isChange = true;
    } else {
      this.isChange = false;
    }
  }
  onChangeChart() {
    if (this.items && this.isChangeChart !== null) {
      this.showChart();
    }
  }
  checkChanged() {
    if (this.fromDate && this.toDate) {
      this.isChange = true;
    } else {
      this.isChange = false;
    }
  }
  showChart() {
    this.blockedPanelChart = true;
    if (this.items && this.items.length > 0 && this.isChangeChart === true) {
              this.data = {
          labels: this.lstDate,
          datasets: [
              {
                  label: 'Số lượng người dùng đăng ký',
                  data: this.lstCount,
                  backgroundColor: '#9CCC65',
                  borderColor: '#7CB342',
              }
          ]
      };
    } else if (this.items && this.items.length > 0 && this.isChangeChart === false) {
      this.data = {
        labels: this.lstDate,
        datasets: [
            {
                label: 'Số lượng người dùng đăng ký',
                data: this.lstCount,
                fill: false,
                borderColor: '#4bc0c0'
            }
        ],
        lines: {
          fillColor: 'rgba(150, 202, 89, 0.12)'
      },
      };
    }
    setTimeout(() => { this.blockedPanelChart = false; }, 1000);
  }
  loadNewRegisters() {
    this.blockedPanel = true;
    const from = this.datePipe.transform(this.fromDate, 'yyyy/MM/dd');
    const to = this.datePipe.transform(this.toDate, 'yyyy/MM/dd');
    this.subscription.add(this.statisticsService.getNewRegisters(this.isChooseType, from, to)
      .subscribe((response: Statistic[]) => {
        this.processLoadData(response);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(response: Statistic[]) {
    this.items = response;
    this.lstCount = [];
    this.lstDate = [];
    response.map(data => {
      this.lstCount.push(data.numberOfValue);
      this.lstDate.push(this.datePipe.transform(data.date, 'dd/MM/yyyy'));
    });
    this.totalRecords = response.length;
    this.isChange = false;
    this.showChart();
    setTimeout(() => { this.blockedPanel = false; }, 1000);
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
