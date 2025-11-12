import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { AppComponent } from './app/app.js';
import { config } from './app/app.config.server.js';

const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(AppComponent, {
    ...config,
    providers: [
      ...(config.providers || []),
      provideHttpClient(withFetch()) 
    ]
  }, context);

export default bootstrap;
