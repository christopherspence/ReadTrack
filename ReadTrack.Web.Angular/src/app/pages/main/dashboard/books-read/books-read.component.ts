import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
    selector: 'app-books-read',
    templateUrl: './books-read.component.html',
    styleUrls: ['./books-read.component.css'],
    standalone: true,
    imports: [
        BaseChartDirective,
        CommonModule,
        MatCardModule
    ]
})
export class BooksReadComponent {
  public barChartData: ChartData<'bar'> = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [
      {
        data: [65, 59, 80, 81, 56, 55, 40],
        label: 'Series A',
        // 2. Styling attributes specific to bars
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
  
  public barChartLegend = true;  
  public barChartType: ChartType = 'bar'; 
}