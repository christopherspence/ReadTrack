import { Component } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData, ChartType } from 'chart.js';
import 'chart.js/auto'; 
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-reading-time',
  templateUrl: './reading-time.component.html',
  styleUrls: ['./reading-time.component.css'],
  standalone: true,
  imports: [
    BaseChartDirective,
    MatCardModule
  ]
})
export class ReadingTimeComponent {
  public lineChartData: ChartData<'line'> = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [
      {
        data: [65, 59, 80, 81, 56, 55, 40],
        label: 'Series A',
        fill: false,
        borderColor: '#3f51b5', // Optional: Gives your line a clear theme color
        tension: 0.1             // Optional: Gives the line a slight smooth curve
      }
    ]
  };

  public lineChartOptions = {
    responsive: true
  };
  
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line'; 
}
