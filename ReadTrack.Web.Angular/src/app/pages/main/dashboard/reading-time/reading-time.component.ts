import { Component } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData, ChartType } from 'chart.js';
import 'chart.js/auto'; 
import { MatCardModule } from '@angular/material/card';
import { CommonModule, DatePipe } from '@angular/common';
import { AnalyticsService } from '../../../../core/services';

@Component({
  selector: 'app-reading-time',
  templateUrl: './reading-time.component.html',
  styleUrls: ['./reading-time.component.css'],
  standalone: true,
  imports: [
    BaseChartDirective,
    CommonModule,
    MatCardModule
  ],
  providers: [DatePipe]
})
export class ReadingTimeComponent {
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
      const data = await this.service.readingTime(sevenDaysAgo, today).toPromise();
  
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
          fill: true,
          borderColor: '#3f51b5', 
          backgroundColor: 'rgba(63, 81, 181, 0.2)',
          tension: 0.1            
        }
      ]
    };

    public chartOptions = {
      responsive: true
    };
    
    public chartLegend = false;
    public chartType: ChartType = 'line'; 
}
