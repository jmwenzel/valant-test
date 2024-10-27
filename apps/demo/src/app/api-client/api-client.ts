import { Observable, throwError, of } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { catchError, mergeMap } from 'rxjs/operators';

export module ValantDemoApiClient {
    export const API_BASE_URL = new InjectionToken<string>('BASE_API_URL');

    @Injectable()
    export class Client {
        private apiUrl: string;

        constructor(
            @Inject(HttpClient) private httpClient: HttpClient, 
            @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
            this.apiUrl = baseUrl || "";
        }

        private generateOptions(method: string, payload?: any): { body?: string; headers: HttpHeaders; observe: 'response'; responseType: 'blob' } {
            const headers = new HttpHeaders({
                "Content-Type": payload ? "application/json" : undefined,
                "Accept": "text/plain"
            });

            return {
                body: payload ? JSON.stringify(payload) : undefined,
                observe: "response",
                responseType: "blob",
                headers
            };
        }

        private processResponse<T>(response: HttpResponse<Blob>): Observable<T> {
            const statusCode = response.status;
            const responseBody = response.body;

            if (statusCode === 200) {
                return convertBlobToText(responseBody).pipe(
                    mergeMap(text => {
                        return of(text === "" ? null : JSON.parse(text));
                    })
                );
            } else {
                return convertBlobToText(responseBody).pipe(
                    mergeMap(text => {
                        return createError("Something went wrong. Please try again.", statusCode, text, response.headers);
                    })
                );
            }
        }

        getMazes(body?: MazeRequest): Observable<MazeResponse> {
            const endpoint = `${this.apiUrl}/api/mazes`;
            const options = this.generateOptions("post", body);

            return this.httpClient.request("post", endpoint, options).pipe(
                mergeMap(response => this.processResponse<MazeResponse>(response)),
                catchError(response => this.handleError<MazeResponse>(response))
            );
        }

        getAvailableMoves(id: string | null | undefined, position: number): Observable<string[]> {
            id = id || '-1';
            position = position || -1;

            const endpoint = `${this.apiUrl}/api/maze/${encodeURIComponent(id)}/availableMoves/${encodeURIComponent(position)}`;
            const options = this.generateOptions("get");

            return this.httpClient.request("get", endpoint, options).pipe(
                mergeMap(response => this.processResponse<string[]>(response)),
                catchError(response => this.handleError<string[]>(response))
            );
        }

        getMaze(id: string | null): Observable<string[]> {
            if (id === undefined || id === null) {
                throw new Error("Missing value.");
            }

            const endpoint = `${this.apiUrl}/api/maze/${encodeURIComponent(id)}`;
            const options = this.generateOptions("get");

            return this.httpClient.request("get", endpoint, options).pipe(
                mergeMap(response => this.processResponse<string[]>(response)),
                catchError(response => this.handleError<string[]>(response))
            );
        }

        uploadMaze(body?: FileRequest): Observable<boolean> {
            const endpoint = `${this.apiUrl}/api/mazefile`;
            const options = this.generateOptions("post", body);

            return this.httpClient.request("post", endpoint, options).pipe(
                mergeMap(response => this.processResponse<boolean>(response)),
                catchError(response => this.handleError<boolean>(response))
            );
        }

        private handleError<T>(response: any): Observable<T> {
            if (response instanceof HttpResponse) {
                return this.processResponse<T>(response);
            }
            return throwError(response);
        }
    }

    export interface MazeRequest {
        startIndex?: number;
        size?: number;
    }

    export interface MazeResponse {
        total?: number;
        items?: string[];
    }

    export interface FileRequest {
        fileName?: string;
        mazeFile?: string[];
    }

    class ApiError extends Error {
        constructor(public message: string, public status: number, public response: string, public headers: { [key: string]: any }, public result: any) {
            super(message);
        }

        static isApiError(obj: any): obj is ApiError {
            return obj instanceof ApiError;
        }
    }

    function createError(message: string, status: number, response: string, headers: { [key: string]: any }, result?: any): Observable<any> {
        return throwError(result ?? new ApiError(message, status, response, headers, null));
    }

    function convertBlobToText(blob: Blob): Observable<string> {
        return new Observable<string>(observer => {
            if (!blob) {
                observer.next("");
                observer.complete();
            } else {
                const reader = new FileReader();
                reader.onload = event => {
                    observer.next(event.target?.result as string);
                    observer.complete();
                };
                reader.readAsText(blob);
            }
        });
    }
}