import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { AnalyticsService } from 'src/app/core/services';

@Component({
    selector: 'app-pages-read',
    templateUrl: './pages-read.component.html',
    styleUrls: ['./pages-read.component.css'],
    standalone: true,
    imports: [
        BaseChartDirective,
        CommonModule,        
        MatCardModule
    ],
    providers: [DatePipe]
})
export class PagesReadComponent implements OnInit {
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
    const data = await this.service.pagesRead(sevenDaysAgo, today).toPromise();

    if (data?.length) {
      data.forEach(d => {
        this.labels.push(this.datePipe.transform(d.date, 'MM-dd-yyyy') ?? '');
        this.data.push(d.value ?? 0);
      });      
    }

    this.dataLoaded = true;
  }

  public chartData: ChartData<'line'> = {
      labels: this.labels,
      datasets: [
        {
          data: this.data,        
          fill: false,
          borderColor: '#3f51b5', // Optional: Gives your line a clear theme color
          tension: 0.1             // Optional: Gives the line a slight smooth curve
        }
      ]
    };

    public chartOptions = {
      responsive: true
    };
    
    public chartLegend = false;
    public chartType: ChartType = 'line';
  
}