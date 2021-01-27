import { TestBed } from '@angular/core/testing';

import { ReservationSystemApiService } from './reservation-system-api.service';

describe('ReservationSystemApiService', () => {
  let service: ReservationSystemApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReservationSystemApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
