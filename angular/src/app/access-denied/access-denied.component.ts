import { Component } from '@angular/core';

@Component({
    selector: 'app-access-denied',
    template: `
    <div class="container text-center mt-5">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <div class="card shadow border-danger">
            <div class="card-body p-5">
              <h1 class="display-1 text-danger fw-bold"><i class="fas fa-ban"></i></h1>
              <h2 class="card-title mt-3">Access Denied</h2>
              <p class="card-text lead text-muted mt-3">
                You do not have permission to access the requested resource.
              </p>
              <div class="mt-4">
                <a routerLink="/" class="btn btn-primary btn-lg">
                  <i class="fas fa-home me-2"></i> Go to Home
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
    standalone: false
})
export class AccessDeniedComponent { }
