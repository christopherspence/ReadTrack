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
  public lineChartData: ChartData<'line'> = {
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

  public lineChartOptions = {
    responsive: true
  };
  
  public lineChartLegend = false;
  public lineChartType: ChartType = 'line'; 
}
