import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MazeService } from '../services/maze.service';
import { ValantDemoApiClient } from '../api-client/api-client';

@Component({
  selector: 'valant-maze-upload',
  templateUrl: './maze-upload.component.html',
  styleUrls: ['./maze-upload.component.less'],
})
export class MazeUploadComponent implements OnInit {
  allowedFileTypes = ['text/plain'];
  mazeFile: File | null = null;
  hasWarnings = false;

  @Output() mazeAdded = new EventEmitter<boolean>();

  constructor(private mazeService: MazeService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {}

  async onFileChange(files: FileList): Promise<void> {
    this.resetState();
    if (files.length === 0) {
      this.showAlert('Select file to upload.', 'error-snackbar');
      return;
    }

    const file = files[0];
    if (file) {
      if (!this.isValidFileType(file)) {
        this.showAlert('Invalid format.', 'error-snackbar');
        return;
      }
      if (!(await this.isValidContent(file))) return;
      this.mazeFile = file;
    }
  }

  async handleGenerateClick(): Promise<void> {
    if (!this.mazeFile) {
      this.showAlert('Select file to upload', 'error-snackbar');
      return;
    }
    await this.uploadFile(this.mazeFile);
  }

  private async uploadFile(mazeFile: File): Promise<void> {
    const lines = await this.getFileText(mazeFile);

    const request: ValantDemoApiClient.FileRequest = {
      fileName: mazeFile.name,
      mazeFile: lines,
    };

    this.mazeService.uploadMaze(request).subscribe({
      next: (response: boolean) => {
        this.handleUploadResponse(response);
      },
      error: (error) => {
        this.showAlert(`Error while uploading file: ${error}`, 'error-snackbar');
      },
    });
  }

  private handleUploadResponse(response: boolean): void {
    if (response) {
      this.showAlert('File uploaded!', 'success-snackbar');
      this.mazeAdded.emit(true);
    } else {
      this.showAlert('Invalid format.', 'error-snackbar');
    }
  }

  private async isValidContent(mazeFile: File): Promise<boolean> {
    const lines = await this.getFileText(mazeFile);
    const isFormatValid = this.validateFormat(lines);
    const hasSameLength = this.checkLineLengths(lines);

    if (!isFormatValid || !hasSameLength) {
      this.hasWarnings = true;
    }
    
    return isFormatValid;
  }

  private validateFormat(lines: string[]): boolean {
    const pattern = /^[\sSOXE]*$/i;
    const isValid = lines.every((item) => pattern.test(item));
    if (!isValid) {
      this.showAlert('Invalid file content', 'error-snackbar');
    }
    return isValid;
  }

  private checkLineLengths(lines: string[]): boolean {
    const maxLength = Math.max(...lines.map(line => line.trim().length));
    const hasSameLength = lines.every((item) => item.trim().length === maxLength);
    if (!hasSameLength) {
      this.showAlert('Blanks will be filled with X.', 'warning-snackbar');
    }
    return hasSameLength;
  }

  private async getFileText(mazeFile: File): Promise<string[]> {
    const text = await this.readFile(mazeFile);
    return text.split('\n').map(line => line.replace(/\r/g, ''));
  }

  private readFile(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = reject;
      reader.readAsText(file);
    });
  }

  private showAlert(message: string, className: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 2000,
      verticalPosition: 'top',
      panelClass: [className],
    });
  }

  private resetState(): void {
    this.mazeFile = null;
    this.hasWarnings = false;
  }

  private isValidFileType(file: File): boolean {
    return this.allowedFileTypes.includes(file.type);
  }
}
