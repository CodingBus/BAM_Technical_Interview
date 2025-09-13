import { TestBed } from '@angular/core/testing';

import { AstronautDuty } from './astronaut-duty';

describe('AstronautDuty', () => {
  let service: AstronautDuty;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AstronautDuty);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
