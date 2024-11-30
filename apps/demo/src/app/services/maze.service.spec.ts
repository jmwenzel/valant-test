import { TestBed } from '@angular/core/testing';
import { MazeService } from './maze.service';
import { ValantDemoApiClient } from '../api-client/api-client';
import { of } from 'rxjs';

describe('MazeService', () => {
  let service: MazeService;
  let mockHttpClient: any;

  beforeEach(() => {
    mockHttpClient = {
      getAvailableMoves: jest.fn(),
      getMazes: jest.fn(),
      uploadMaze: jest.fn(),
      getMaze: jest.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        MazeService,
        { provide: ValantDemoApiClient.Client, useValue: mockHttpClient },
      ],
    });

    service = TestBed.inject(MazeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAvailableMoves', () => {
    it('should call getAvailableMoves on the httpClient', () => {
      const id = 'maze1';
      const pos = 1;
      const expectedMoves = ['UP', 'DOWN'];

      mockHttpClient.getAvailableMoves.mockReturnValue(of(expectedMoves));

      service.getAvailableMoves(id, pos).subscribe(moves => {
        expect(moves).toEqual(expectedMoves);
      });

      expect(mockHttpClient.getAvailableMoves).toHaveBeenCalledWith(id, pos);
    });
  });

  describe('getAllMazes', () => {
    it('should call getMazes on the httpClient', () => {
      const request = { startIndex: 0, size: 10 };
      const expectedResponse = { total: 1, items: ['maze1', 'maze2'] };

      mockHttpClient.getMazes.mockReturnValue(of(expectedResponse));

      service.getAllMazes(request).subscribe(response => {
        expect(response).toEqual(expectedResponse);
      });

      expect(mockHttpClient.getMazes).toHaveBeenCalledWith(request);
    });
  });

  describe('uploadMaze', () => {
    it('should call uploadMaze on the httpClient', () => {
      const request = { fileName: 'maze.txt', mazeFile: ['XX', 'OO'] };
      const expectedResponse = true;

      mockHttpClient.uploadMaze.mockReturnValue(of(expectedResponse));

      service.uploadMaze(request).subscribe(response => {
        expect(response).toBe(expectedResponse);
      });

      expect(mockHttpClient.uploadMaze).toHaveBeenCalledWith(request);
    });
  });

  describe('getMazeById', () => {
    it('should call getMaze on the httpClient', () => {
      const id = 'maze1';
      const expectedMaze = ['XOX', 'O O', 'XOX'];

      mockHttpClient.getMaze.mockReturnValue(of(expectedMaze));

      service.getMazeById(id).subscribe(maze => {
        expect(maze).toEqual(expectedMaze);
      });

      expect(mockHttpClient.getMaze).toHaveBeenCalledWith(id);
    });
  });
});
