import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MazeIndexComponent } from './maze-index.component';

describe('MazeIndexComponent', () => {
  let component: MazeIndexComponent;
  let fixture: ComponentFixture<MazeIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MazeIndexComponent],
    }).compileComponents(); 

    fixture = TestBed.createComponent(MazeIndexComponent); 
    component = fixture.componentInstance; 
    fixture.detectChanges(); 
  });

  it('should create the component', () => {
    expect(component).toBeTruthy(); 
  });

  it('should render the maze upload component', () => {
    const compiled = fixture.nativeElement; 
    expect(compiled.querySelector('valant-maze-upload')).toBeTruthy(); 
  });
});
