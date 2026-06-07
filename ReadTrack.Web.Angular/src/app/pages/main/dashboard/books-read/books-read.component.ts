import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { AnalyticsService } from 'src/app/core/services';

@Component({
    selector: 'app-books-read',
    templateUrl: './books-read.component.html',
    styleUrls: ['./books-read.component.css'],
    standalone: true,
    imports: [
        BaseChartDirective,
        CommonModule,        
        MatCardModule
    ],
    providers: [DatePipe]
})
export class BooksReadComponent implements OnInit {
  private labels = new Array<string>();

  private data = new Array<number>();

  dataLoaded = false;

  constructor(
    private datePipe: DatePipe,
    private service: AnalyticsService) {}
  
  async ngOnInit(): Promise<void> {
    const today = new Date();
    const sevenDaysAgo = new Date(today); // Create a copy to avoid mutating 'today'

    sevenDaysAgo.setDate(today.getDate() - 6);
    const data = await this.service.booksRead(sevenDaysAgo, today).toPromise();

    if (data?.length) {
      data.forEach(d => {
        this.labels.push(this.datePipe.transform(d.date, 'MM-dd-yyyy') ?? '');
        this.data.push(d.value ?? 0);
      });      
    }

    this.dataLoaded = true;
  }

  public barChartData: ChartData<'bar'> = {
    labels: this.labels,
    datasets: [
      {
        data: this.data,
        backgroundColor: '#3f51b5',     // Solid color for the bars
        hoverBackgroundColor: '#283593' // Color when hovering over bars
      }
    ]
  };

  public barChartOptions = {
    responsive: true,
    scales: {
      y: {
        beginAtZero: true // Ensures the chart axis starts at 0 instead of auto-scaling
      }
    }
  };
  
  public barChartLegend = false;  
  public barChartType: ChartType = 'bar'; 
  
}