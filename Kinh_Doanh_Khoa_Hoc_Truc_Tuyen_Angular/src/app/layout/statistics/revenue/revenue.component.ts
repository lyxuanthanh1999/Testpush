import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { StatisticsService } from '../../../shared/services/statistics.service';
import { Revenue } from '../../../shared/models/revenue.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-revenue',
  templateUrl: './revenue.component.html',
  styleUrls: ['./revenue.component.scss']
})
export class RevenueComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public blockedPanel = false;
  public blockedPanelDetails = false;
  public blockedPanelChart = false;
  /**
   * Date
   */
  public vi: any;
  public lstProfit = [];
  public lstRevenue = [];
  public lstDate = [];
  public fromDate: any;
  public toDate: any;
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
    {value: null, label: 'Chọn biệu đồ'},
    {value: true, label: 'Bar'},
    {value: false, label: 'Line'}
  ];
   /**
   * Table
   */
  public totalRecords = 0;
  public items: Revenue[];
  public showDetails = false;
  public details: Revenue[] = [];
  public totalDetailsRecords: number;
  public data: any;
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
    this.details = null;
    this.totalDetailsRecords = 0;
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
    if (!this.isChange && this.fromDate && this.toDate) {
      this.isChange = true;
    } else {
      this.isChange = false;
    }
  }
  loadRevenueDaily() {
    this.blockedPanel = true;
    const from = this.datePipe.transform(this.fromDate, 'yyyy/MM/dd');
    const to = this.datePipe.transform(this.toDate, 'yyyy/MM/dd');
    this.subscription.add(this.statisticsService.getRevenueDaily(this.isChooseType, from, to)
      .subscribe((response: Revenue[]) => {
        this.processLoadData(response);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(response: Revenue[]) {
    this.items = response;
    this.lstProfit = [];
    this.lstRevenue = [];
    this.lstDate = [];
    response.map(data => {
      this.lstProfit.push(data.profit);
      this.lstRevenue.push(data.revenue);
      this.lstDate.push(this.datePipe.transform(data.date, 'dd/MM/yyyy'));
    });
    this.totalRecords = response.length;
    this.isChange = false;
    this.showHideDetailsTable();
    this.showChart();
    setTimeout(() => { this.blockedPanel = false; }, 1000);
  }
  showChart() {
    this.blockedPanelChart = true;
    if (this.items && this.items.length > 0 && this.isChangeChart === true) {
              this.data = {
          labels: this.lstDate,
          datasets: [
              {
                  label: 'Doanh thu',
                  data: this.lstRevenue,
                  backgroundColor: '#42A5F5',
                  borderColor: '#1E88E5'
              },
              {
                  label: 'Lợi nhuận',
                  data: this.lstProfit,
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
                label: 'Doanh thu',
                data: this.lstRevenue,
                fill: false,
                borderColor: '#4bc0c0'
            },
            {
                label: 'Lợi nhuận',
                data: this.lstProfit,
                fill: false,
                borderColor: '#d10f1f'
            }
        ],
        lines: {
          fillColor: 'rgba(150, 202, 89, 0.12)'
      },
      };
    }
    setTimeout(() => { this.blockedPanelChart = false; }, 1000);
  }
  showHideDetailsTable() {
    if (this.showDetails && this.items.length > 0) {
      this.loadCountSalesDaily();
    }
  }
  loadCountSalesDaily() {
    this.blockedPanelDetails = true;
    const from = this.datePipe.transform(this.fromDate, 'yyyy/MM/dd');
    const to = this.datePipe.transform(this.toDate, 'yyyy/MM/dd');
    this.statisticsService.getCountSalesDaily(this.isChooseType, from, to).subscribe((response: Revenue[]) => {
      this.details = response;
      this.totalDetailsRecords = response.length;
      setTimeout(() => { this.blockedPanelDetails = false; }, 1000);
    }, () => {
      setTimeout(() => { this.blockedPanelDetails = false; }, 1000);
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
