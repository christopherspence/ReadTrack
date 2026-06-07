import { Component } from '@angular/core';
import { ChartData, ChartType } from 'chart.js';
import { BooksReadComponent } from './books-read/books-read.component';
import { ReadingTimeComponent } from './reading-time/reading-time.component';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css'],    
    standalone: true,
    imports: [BooksReadComponent, ReadingTimeComponent]
})
export class DashboardComponent {
  public lineChartData: ChartData<'line'> = {
    datasets: [
      {
        data: [65, 59, 80, 81, 56, 55, 40],
        label: 'Series A',
        fill: false
      }
    ]
  };
  public lineChartLabels: string[] = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July'
  ];
  public lineChartOptions = {
    responsive: true
  };
  public lineChartColors = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,255,0,0.28)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
}