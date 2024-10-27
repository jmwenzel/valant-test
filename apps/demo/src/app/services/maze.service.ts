import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ValantDemoApiClient } from '../api-client/api-client';

@Injectable({
  providedIn: 'root',
})
export class MazeService {
  constructor(private httpClient: ValantDemoApiClient.Client) {}

  public getAvailableMoves(id:string, pos:number): Observable<string[]> {
    return this.httpClient.getAvailableMoves(id, pos);
  }

  public getAllMazes(request: ValantDemoApiClient.MazeRequest): Observable<ValantDemoApiClient.MazeResponse> {
    return this.httpClient.getMazes(request);
  }

  public uploadMaze(request: ValantDemoApiClient.FileRequest): Observable<boolean> {
    return this.httpClient.uploadMaze(request);
  }

  public getMazeById(id: string): Observable<string[]> {
    return this.httpClient.getMaze(id);
  }
}
