import { Directive, ElementRef, OnInit, Renderer2, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Directive({
  selector: 'input[ssrSafe], textarea[ssrSafe], select[ssrSafe]',
  standalone: true
})
export class SsrSafeInputDirective implements OnInit {
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  private platformId = inject(PLATFORM_ID);

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      // Solo en el navegador
      this.setupClientSideInputs();
    } else {
      // En el servidor
      this.setupServerSideInputs();
    }
  }

  private setupClientSideInputs() {
    const element = this.el.nativeElement;
    
    // Prevenir autofill styling
    this.renderer.setAttribute(element, 'autocomplete', 'off');
    
    // Asegurar estilos consistentes
    this.renderer.addClass(element, 'ssr-safe-input');
  }

  private setupServerSideInputs() {
    const element = this.el.nativeElement;
    
    // Configuración básica para server
    this.renderer.addClass(element, 'ssr-safe-input');
    this.renderer.setAttribute(element, 'autocomplete', 'off');
  }
}