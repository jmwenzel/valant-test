import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { LoggingService } from './logging/logging.service';
import { environment } from '../environments/environment';
import { ValantDemoApiClient } from './api-client/api-client';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { MazeIndexComponent } from './maze-index/maze-index.component';
import { MazeUploadComponent } from './maze-upload/maze-upload.component';
import { MazeService } from './services/maze.service';

export function getBaseUrl(): string {
  return environment.baseUrl;
}

const routes: Routes = [
  { path: '', component: MazeIndexComponent },
];
@NgModule({
  declarations: [AppComponent, MazeUploadComponent, MazeIndexComponent],
  imports: [
    BrowserModule, 
    FormsModule, 
    HttpClientModule,
    RouterModule.forRoot(routes),
  ],
  providers: [
    LoggingService,
    MazeService,
    ValantDemoApiClient.Client,
    { provide: ValantDemoApiClient.API_BASE_URL, useFactory: getBaseUrl },
  ],
  bootstrap: [AppComponent],
  exports: [RouterModule],
})
export class AppModule {}
