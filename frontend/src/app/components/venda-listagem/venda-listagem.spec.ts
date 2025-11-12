import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendaListagem } from './venda-listagem';

describe('VendaListagem', () => {
  let component: VendaListagem;
  let fixture: ComponentFixture<VendaListagem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VendaListagem]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VendaListagem);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
