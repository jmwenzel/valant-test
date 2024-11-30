import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MazeUploadComponent } from './maze-upload.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MazeService } from '../services/maze.service';
import { FormsModule } from '@angular/forms'

describe('MazeUploadComponent', () => {
  let component: MazeUploadComponent;
  let fixture: ComponentFixture<MazeUploadComponent>;
  let mockMazeService: any;
  let mockSnackBar: any;

  beforeEach(async () => {
    mockMazeService = {
      uploadMaze: jest.fn(),
    };

    mockSnackBar = {
      open: jest.fn(),
    };

    await TestBed.configureTestingModule({
      declarations: [MazeUploadComponent],
      imports: [FormsModule],
      providers: [
        { provide: MazeService, useValue: mockMazeService },
        { provide: MatSnackBar, useValue: mockSnackBar },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MazeUploadComponent);
    component = fixture.componentInstance;

    // Detectar cambios para inicializar el componente correctamente
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('handleGenerateClick', () => {
    it('should show an alert if no maze file is uploaded', async () => {
      component.mazeFile = null;
      await component.handleGenerateClick();
      expect(mockSnackBar.open).toHaveBeenCalledWith('Select file to upload', 'Close', expect.any(Object));
    });
  });
});
